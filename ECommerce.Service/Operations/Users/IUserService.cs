using ECommerce.Service.Operations.Users.Dtos;
using ECommerce.Service.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Operations.Users
{
    public interface IUserService
    {
        Task<ServiceMassage> AddAsync(UserAddDto dto);
        Task<ServiceMassage> UpdateAsync(UserUpdateDto dto);
        Task<ServiceMassage> ChangePasswordAsync(UserChangePasswordDto dto);
        Task<ServiceMassage> ForgotPasswordAsync(UserForgotPasswordDto dto);
        Task<ServiceMassage> ResetPasswordAsync(UserResetPasswordDto dto);
        Task<ServiceMassage> Delete(int id);
        Task<ServiceMassage> LoginAsync(LoginUserDto dto);
        Task<ServiceMassage<UserInfoDto>> GetByIdAsync(int id);
    }
}
