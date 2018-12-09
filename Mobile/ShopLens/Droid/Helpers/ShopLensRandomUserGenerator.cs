using RandomNameGenerator;
using ShopLensWeb;
using System;
using System.Security.Cryptography;

namespace ShopLens
{
    public static class ShopLensRandomUserGenerator
    {
        static Random randomNumGenerator = new Random();

        public static User GenerateRandomUser(string userGuid, int minimumAge, int maximumAge, bool onlyMale = false, bool onlyFemale = false)
        {
            Gender userGender;

            var randomYear = randomNumGenerator.Next(DateTime.Now.Year - maximumAge, DateTime.Now.Year - minimumAge);
            var randomMonth = randomNumGenerator.Next(1, 13);
            var randomDay = randomNumGenerator.Next(1, DateTime.DaysInMonth(randomYear, randomMonth) + 1);

            var randomBirthday = new DateTime(randomYear, randomMonth, randomDay);

            if (onlyMale)
            {
                userGender = Gender.Male;
            }
            else if (onlyFemale)
            {
                userGender = Gender.Female;
            }
            else
            {
                var genderPickChance = randomNumGenerator.Next(100);

                if (genderPickChance >= 50)
                {
                    userGender = Gender.Male;
                }
                else
                {
                    userGender = Gender.Female;
                }
            }

            var randomName = NameGenerator.GenerateFirstName(userGender);

            // TODO: fix GUID type, so that it's really GUID in the database and not string.

            return new User { Birthday = randomBirthday, Name = randomName, UserId = userGuid };
        }
    }
}