using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BankDataWebAPI.Data;
using BankDataWebAPI.Passwords;
using BankDataWebAPI.Models;

namespace BankDataWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateUser([FromBody] CreateUserData data)
        {
            if(string.IsNullOrWhiteSpace(data.Username) ||
               string.IsNullOrWhiteSpace(data.Name) ||
               string.IsNullOrWhiteSpace(data.Email) ||
               string.IsNullOrWhiteSpace(data.Password))
            {
                return BadRequest(new
                {
                    error = "VALIDATION_ERROR",
                    message = "Username, name, email, and password are required",
                    details = (object)null
                });
            }
            string passwordHash;
            string passwordSalt;

            PasswordManager.CreatePasswordHash(
                data.Password,
                out passwordHash,
                out passwordSalt);

            User user = new User
            {
                UserName = data.Username,
                Name = data.Name,
                Email = data.Email,
                Address = data.Address,
                Phone = data.Phone,
                PicturePath = null,
                PasswordHashed = passwordHash,
                PasswordSalted = passwordSalt,
                IsActive = true
            };

            bool success = DBManager.InsertUser(user);

            if(!success)
            {
                return Conflict(new
                {
                    error = "USER_CREATION_FAILED",
                    message = "The user creation failed",
                    details = (object)null
                });
            }
            User createdUser = DBManager.getUserByUserName(data.Username);

            UserResponse response = new UserResponse
            {
                ID = createdUser.ID,
                UserName = createdUser.UserName,
                Name = createdUser.Name,
                Email = createdUser.Email,
                Address = createdUser.Address,
                Phone = createdUser.Phone,
                PicturePath = createdUser.PicturePath,
                IsActive = createdUser.IsActive
            };
            return StatusCode(201, response);
        }

        [HttpGet("username/{username}")]
        public IActionResult GetUserByUserName(string username)
        {
            User user = DBManager.getUserByUserName(username);

            if(user == null)
            {
                return NotFound(new
                {
                    error = "USER_NOT_FOUND",
                    message = "The user not found",
                    details = (object)null
                });
            }
            return Ok(new UserResponse
            {
                ID = user.ID,
                UserName = user.UserName,
                Name = user.Name,
                Email = user.Email,
                Address = user.Address,
                Phone = user.Phone,
                PicturePath = user.PicturePath,
                IsActive = user.IsActive
            });
        }

        [HttpGet("email/{email}")]
        public IActionResult GetUserByEmail(string email)
        {
            User user = DBManager.GetUserByEmail(email);

            if(user == null)
            {
                return NotFound(new
                {
                    error = "USER_NOT_FOUND",
                    message = "The user not found",
                    details = (object)null
                });
            }
            return Ok(new UserResponse
            {
                ID = user.ID,
                UserName = user.UserName,
                Name = user.Name,
                Email = user.Email,
                Address = user.Address,
                Phone = user.Phone,
                PicturePath = user.PicturePath,
                IsActive = user.IsActive
            });
        }
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UpdateUserData data)
        {
            if (id <= 0 ||
               string.IsNullOrWhiteSpace(data.Name) ||
               string.IsNullOrWhiteSpace(data.Email))
            {
                return BadRequest(new
                {
                    error = "VALIDATION_ERROR",
                    message = "Enter valid user ID, Name, and Email",
                    details = (object)null
                });
            }

            User ExistingUser = DBManager.GetUserByID(id);

            if (ExistingUser == null)
            {
                return NotFound(new
                {
                    error = "USER_NOT_FOUND",
                    message = "The user not found",
                    details = (object)null
                });
            }
            if (!ExistingUser.IsActive)
            {
                return Conflict(new
                {
                    error = "USER_INACTIVE",
                    message = "A deactivated user can't be update",
                    details = (object)null
                });
            }
            ExistingUser.Name = data.Name;
            ExistingUser.Email = data.Email;
            ExistingUser.Address = data.Address;
            ExistingUser.Phone = data.Phone;

            bool success = DBManager.UpdateUser(ExistingUser);

            if (!success)
            {
                return Conflict(new
                {
                    error = "UPDATE_FAILED",
                    message = "The profile can't be updated",
                    details = (object)null
                });
            }
            return Ok(new UserResponse
            {
                ID = ExistingUser.ID,
                UserName = ExistingUser.UserName,
                Name = ExistingUser.Name,
                Email = ExistingUser.Email,
                Address = ExistingUser.Address,
                Phone = ExistingUser.Phone,
                PicturePath = ExistingUser.PicturePath,
                IsActive = ExistingUser.IsActive
            });
        }
        [HttpDelete("{id}")]
        public IActionResult DeactivateUser(int id)
        {
            if(id <= 0)
            {
                return BadRequest(new
                {
                    error = "INVALID_USER_ID",
                    message = "Enter a valid user ID",
                    details = (object)null
                });
            }
            User ExistingUser = DBManager.GetUserByID(id);

            if(ExistingUser == null)
            {
                return NotFound(new
                {
                    error = "USER_NOT_FOUND",
                    message = "The user not found",
                    details = (object)null
                });
            }
            if(!ExistingUser.IsActive)
            {
                return Conflict(new
                {
                    error = "USER_INACTIVE",
                    message = "The user is aloready deactivated",
                    details = (object)null
                });
            }
            bool success = DBManager.DeactivateUser(id);

            if(!success)
            {
                return StatusCode(500, new
                {
                    error = "DEACTIVATION_FAILED",
                    message = "The user can't be deactivated",
                    details = (object)null
                });
            }
            return Ok(new
            {
                message = "User deactivated successfully"
            });
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginData data)
        {
            if(string.IsNullOrWhiteSpace(data.UserName) || string.IsNullOrWhiteSpace(data.Password))
            {
                return BadRequest(new
                {
                    error = "VALIDATION_ERROR",
                    message = "Enter the username and password",
                    details = (object)null
                });
            }
            User user = DBManager.getUserByUserName(data.UserName);

            if (user == null ||
                !user.IsActive ||
                !PasswordManager.VerifyPassword(
                    data.Password,
                    user.PasswordHashed,
                    user.PasswordSalted))
            {
                return Unauthorized(new
                {
                    error = "INVALID_CREDENTIALS",
                    message = "The username or password is incorrect",
                    details = (object)null
                });
            }

            return Ok(new
            {
                message = "Login successful",
                user = new UserResponse
                {
                    ID = user.ID,
                    UserName = user.UserName,
                    Name = user.Name,
                    Email = user.Email,
                    Address = user.Address,
                    Phone = user.Phone,
                    PicturePath = user.PicturePath,
                    IsActive = user.IsActive
                }
            });
        }
        [HttpPost("{id}/picture")]
        public IActionResult UploadPicture(int id, IFormFile picture)
        {
            if(picture == null || picture.Length == 0)
            {
                return BadRequest(new
                {
                    error = "INVALID_PICTURE",
                    message = "Select a picture to upload",
                    details = (object)null
                });
            }
            if(picture.Length > 5 * 1024 * 1024)
            {
                return BadRequest(new
                {
                    error = "FILE_TOO_LARGE",
                    message = "Picture can't exceed 5MB",
                    details = (object)null
                });
            }
            string extension = Path.GetExtension(picture.FileName).ToLowerInvariant();

            string[] allowedExtensions =
            {
                ".jpg",
                ".jpeg",
                ".png",
                ".webp"
            };
            if(!allowedExtensions.Contains(extension))
            {
                return BadRequest(new
                {
                    error = "INVALID_FILE_TYPE",
                    message = "Only the JPG, JPEG, and WEBP pictures are allowed",
                    details = (object)null
                });
            }
            User user = DBManager.GetUserByID(id);

            if(user == null)
            {
                return NotFound(new
                {
                    error = "USER_NOT_FOUND",
                    message = "The user was not found",
                    details = (object)null
                });
            }
            if(!user.IsActive)
            {
                return Conflict(new
                {
                    error = "USER_INACTIVE",
                    message = "Inactive user can't update picture",
                    details = (object)null
                });
            }
            string ImageFolder = Path.Combine(Directory.GetCurrentDirectory(), "Images");

            Directory.CreateDirectory(ImageFolder);

            string fileName = Guid.NewGuid().ToString("N") + extension;

            string filePath = Path.Combine(ImageFolder, fileName);

            using(FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                picture.CopyTo(stream);
            }

            user.PicturePath = "Images/" + fileName;

            if(!DBManager.UpdateUser(user))
            {
                if(System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                return StatusCode(500, new
                {
                    error = "PICTURE_UPLOAD_FAILED",
                    message = "The picture can't be saved",
                    details = (object)null
                });
            }

            return Ok(new
            {
                message = "Profile picture uploaded successfully",
                picturePath = user.PicturePath
            });

        }
    }
}
