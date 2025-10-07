using ECommerce.Core.Entities;
using ECommerce.Data.UnitOfWork;
using ECommerce.Service.DataProtector;
using ECommerce.Service.Operations.Users.Dtos;
using ECommerce.Service.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Operations.Users
{
    public class UserManager : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDataProtection _dataProtector;

        public UserManager(IUnitOfWork unitOfWork, IDataProtection dataProtection)
        {
            _unitOfWork = unitOfWork;
            _dataProtector = dataProtection;
        }
        public async Task<ServiceMassage> AddAsync(UserAddDto dto)
        {
            try
            {
                var repo = _unitOfWork.GetRepository<User>();

                var existing = (await repo.GetAllAsync(x => x.Email == dto.Email)).FirstOrDefault();

                if (existing != null)
                {
                    return new ServiceMassage<UserInfoDto>
                    {
                        IsSucced = false,
                        Massage = "Bu email zaten kayıtlı"
                    };
                }

                var user = new User
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    PasswordHash = _dataProtector.Protected(dto.Password),
                    PhoneNumber = dto.PhoneNumber,
                    CreatedDate = DateTime.UtcNow
                };

                await _unitOfWork.BeginTransactionAsync();

                await repo.AddAsync(user);

                await _unitOfWork.CompleteAsync();

                await _unitOfWork.CommitAsync();

                return new ServiceMassage<UserInfoDto>
                {
                    IsSucced = true,
                    Massage = "Kullanıcı Başarıyla oluşturuldu",
                    Data = new UserInfoDto
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                    }
                };
            }
            catch (Exception)
            {

                await _unitOfWork.RollbackAsync();
                return new ServiceMassage<UserInfoDto>
                {
                    IsSucced = false,
                    Massage = "beklenmeyen bir hata oluştu"
                };
            }
        }

        public async Task<ServiceMassage> ChangePasswordAsync(UserChangePasswordDto dto)
        {
            try
            {
                var repo = _unitOfWork.GetRepository<User>();

                var user = await repo.GetByIdAsync(dto.UserId);

                if (user == null)
                {
                    return new ServiceMassage
                    {
                        IsSucced = false,
                        Massage = "Kullanıcı bulunamadı"
                    };
                }

                var currentPassword = _dataProtector.Unprotected(user.PasswordHash);

                if (currentPassword != dto.CurrentPassword)
                {
                    return new ServiceMassage
                    {
                        IsSucced = false,
                        Massage = "Mevcut şifre yanlış"
                    };
                }

                if (dto.NewPassword != dto.ConfrimPassword)
                {
                    return new ServiceMassage
                    {
                        IsSucced = false,
                        Massage = "Yeni şifreler eşleşmiyor"
                    };
                }

                user.PasswordHash = _dataProtector.Protected(dto.NewPassword);

                user.UpdateDate = DateTime.UtcNow;

                await _unitOfWork.BeginTransactionAsync();
                repo.Update(user);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitAsync();

                return new ServiceMassage
                {
                    IsSucced = true,
                    Massage = "Şifre başarıyla değiştirildi"
                };
            }
            catch (Exception)
            {

                await _unitOfWork.RollbackAsync();
                return new ServiceMassage
                {
                    IsSucced = false,
                    Massage = "Bir hata oluştu"
                };
            }
        }

        public async Task<ServiceMassage> Delete(int id)
        {
            try
            {
                var repo = _unitOfWork.GetRepository<User>();

                var user = await repo.GetByIdAsync(id);

                if (user == null)
                {
                    return new ServiceMassage
                    {
                        IsSucced = false,
                        Massage = "Kullanıcı bulunamadı"
                    };
                }

                await _unitOfWork.BeginTransactionAsync();
                repo.Remove(user);// soft delete
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitAsync();

                return new ServiceMassage
                {
                    IsSucced = true,
                    Massage = "Kullanıcı başarıyla silindi"
                };
            }
            catch (Exception)
            {

                await _unitOfWork.RollbackAsync();

                return new ServiceMassage
                {
                    IsSucced = false,
                    Massage = "Beklenmeyen bir hata oluştu"
                };
            }
        }

        public async Task<ServiceMassage> ForgotPasswordAsync(UserForgotPasswordDto dto)
        {
            try
            {
                var repo = _unitOfWork.GetRepository<User>();

                var user = (await repo.GetAllAsync(u => u.Email == dto.Email)).FirstOrDefault();

                if (user == null)
                {
                    return new ServiceMassage
                    {
                        IsSucced = false,
                        Massage = "Email adresi bulunamadı"
                    };
                }

                string tempPassword = "Temp1234";
                user.PasswordHash = _dataProtector.Protected(tempPassword);

                user.UpdateDate = DateTime.UtcNow;

                await _unitOfWork.BeginTransactionAsync();
                repo.Update(user);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitAsync();

                return new ServiceMassage
                {
                    IsSucced = true,
                    Massage = "Şifre sıfırlama başarılı"
                };
            }
            catch (Exception)
            {

                await _unitOfWork.RollbackAsync();
                return new ServiceMassage
                {
                    IsSucced = false,
                    Massage = "Bir hata oluştu"
                };
            }
        }


        public async Task<ServiceMassage> ResetPasswordAsync(UserResetPasswordDto dto)
        {
            try
            {
                if (dto.NewPassword != dto.ConfirmPassword)
                {
                    return new ServiceMassage
                    {
                        IsSucced = false,
                        Massage = "Şifreler birbiriyle eşleşmiyor."
                    };
                }

                var repo = _unitOfWork.GetRepository<User>();
                var user = (await repo.GetAllAsync(u => u.ResetToken == dto.Token)).FirstOrDefault();

                if (user == null)
                {
                    return new ServiceMassage
                    {
                        IsSucced = false,
                        Massage = "Geçersiz veya süresi dolmuş token."
                    };
                }

                user.PasswordHash = _dataProtector.Protected(dto.NewPassword);
                user.ResetToken = null;
                user.UpdateDate = DateTime.UtcNow;

                await _unitOfWork.BeginTransactionAsync();
                repo.Update(user);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitAsync();

                return new ServiceMassage
                {
                    IsSucced = true,
                    Massage = "Şifre başarıyla sıfırlandı"
                };
            }
            catch (Exception)
            {

                await _unitOfWork.RollbackAsync();
                return new ServiceMassage
                {
                    IsSucced = false,
                    Massage = "Bir hata oluştu"
                };
            }
        }

        public async Task<ServiceMassage> UpdateAsync(UserUpdateDto dto)
        {
            try
            {
                var repo = _unitOfWork.GetRepository<User>();

                var user = await repo.GetByIdAsync(dto.Id);

                if (user == null)
                {
                    return new ServiceMassage<UserInfoDto>
                    {
                        IsSucced = false,
                        Massage = "Kullanıcı bulunamadı"
                    };
                }

                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                user.Email = dto.Email;
                user.PhoneNumber = dto.PhoneNumber;
                user.UpdateDate = DateTime.UtcNow;

                await _unitOfWork.BeginTransactionAsync();
                repo.Update(user);

                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitAsync();

                return new ServiceMassage<UserInfoDto>
                {
                    IsSucced = true,
                    Massage = "Kullanıcı güncellendi",
                    Data = new UserInfoDto
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                    }
                };
            }
            catch (Exception)
            {

                await _unitOfWork.RollbackAsync();
                return new ServiceMassage<UserInfoDto>
                {
                    IsSucced = false,
                    Massage = "Beklenmeyen bir hata oluştu"
                };
            }
        }
    }
}
