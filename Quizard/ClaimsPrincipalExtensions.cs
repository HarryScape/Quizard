using System.Security.Claims;

namespace Quizard
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Static method extension for retrieving user Id
        /// </summary>
        /// <param name="user"></param>
        /// <returns>string user Id</returns>
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
