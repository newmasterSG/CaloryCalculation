using CaloryCalculatiom.Domain.Entities.Enums;
using CaloryCalculatiom.Domain.Entities;

namespace CaloryCalculation.Application.Builders
{
    public class UserBuilder
    {
        private readonly User _user;

        public UserBuilder()
        {
            _user = new User();
        }

        public UserBuilder WithUserName(string userName)
        {
            _user.UserName = userName;
            return this;
        }

        public UserBuilder WithEmail(string email)
        {
            _user.Email = email;
            return this;
        }

        public UserBuilder WithFirstName(string firstName)
        {
            _user.FirstName = firstName;
            return this;
        }

        public UserBuilder WithLastName(string lastName)
        {
            _user.LastName = lastName;
            return this;
        }

        public UserBuilder WithHeight(double height)
        {
            _user.Height = height;
            return this;
        }

        public UserBuilder WithWeight(double weight)
        {
            _user.Weight = weight;
            return this;
        }

        public UserBuilder WithAge(int age)
        {
            _user.Age = age;
            return this;
        }

        public UserBuilder WithGender(Gender gender)
        {
            _user.Gender = gender;
            return this;
        }

        public UserBuilder WithGoals(ICollection<Goal> goals)
        {
            _user.Goals = goals;
            return this;
        }

        public UserBuilder WithDailyLogs(ICollection<DailyLog> dailyLogs)
        {
            _user.DailyLogs = dailyLogs;
            return this;
        }

        public User Build()
        {
            return _user;
        }
    }
}
