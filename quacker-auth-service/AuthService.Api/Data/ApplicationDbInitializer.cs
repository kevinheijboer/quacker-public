using AuthService.Api.Models;
using AuthService.Api.Payloads.Messages;
using AuthService.Api.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Reflection;

namespace AuthService.Api.Data
{
    public class ApplicationDbInitializer
    {
        public static void SeedRootAdmin(UserManager<ApplicationUser> userManager, IServiceBus serviceBus)
        {
            // Check if there is no user with root role
            if (userManager.GetUsersInRoleAsync(UserRoles.Administrator).Result.Count < 1)
            {
                // If there is no root user

                ApplicationUser rootUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@quacker.nl",
                    CreatedOn = DateTime.Now,
                };

                IdentityResult result = userManager.CreateAsync(rootUser, "Admin123").Result;

                userManager.AddToRoleAsync(rootUser, UserRoles.Administrator).Wait();

                var message = new AccountMessage()
                {
                    UserId = Guid.Parse(rootUser.Id),
                    Username = rootUser.UserName,
                    Email = rootUser.Email,
                    CreatedOn = rootUser.CreatedOn
                };

                serviceBus.SendMessageAsync(message, topicName: "user-registration").Wait();
            }
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in GetRoleConstants(typeof(UserRoles)))
            {
                if (!roleManager.RoleExistsAsync(role.GetValue(null).ToString()).Result)
                {
                    roleManager.CreateAsync(new IdentityRole(role.GetValue(null).ToString())).Wait();
                }
            }
        }

        private static FieldInfo[] GetRoleConstants(System.Type type)
        {
            ArrayList constants = new ArrayList();

            FieldInfo[] fieldInfos = type.GetFields(
                BindingFlags.Public | BindingFlags.Static |
                BindingFlags.FlattenHierarchy);

            foreach (FieldInfo fi in fieldInfos)
                if (fi.IsLiteral && !fi.IsInitOnly)
                    constants.Add(fi);

            // Return an array of FieldInfos
            return (FieldInfo[])constants.ToArray(typeof(FieldInfo));
        }
    }
}
