using SuperariLife.Data.DBRepository.Account;
using SuperariLife.Data.DBRepository.Appointment;
using SuperariLife.Data.DBRepository.City;
using SuperariLife.Data.DBRepository.Country;
using SuperariLife.Data.DBRepository.Coupon;
using SuperariLife.Data.DBRepository.Coupon.RewardCouponCode;
using SuperariLife.Data.DBRepository.Customer;
using SuperariLife.Data.DBRepository.Customer.CustomerAddress;
using SuperariLife.Data.DBRepository.Event;
using SuperariLife.Data.DBRepository.PaymentType;
using SuperariLife.Data.DBRepository.Question;
using SuperariLife.Data.DBRepository.RoleManagement;
using SuperariLife.Data.DBRepository.SettingPage.AboutPage;
using SuperariLife.Data.DBRepository.SettingPage.PrivacyPolicyPage;
using SuperariLife.Data.DBRepository.SettingPage.TermsAndConditionPage;
using SuperariLife.Data.DBRepository.SettingPage.TestimonialReviewPage;
using SuperariLife.Data.DBRepository.State;
using SuperariLife.Data.DBRepository.User;


namespace SuperariLife.Data
{
    public static class DataRegister
    {
        public static Dictionary<Type, Type> GetTypes()
        {
            var dataDictionary = new Dictionary<Type, Type>
            {
                {typeof(IAboutPageRepository), typeof(AboutPageRepository)},
                { typeof(IAccountRepository), typeof(AccountRepository) },
                { typeof(IAppointmentRepository), typeof(AppointmentRepositpory) },
                { typeof(ICityRepository), typeof(CityRepository) },
                { typeof(ICountryRepository), typeof(CountryRepository) },
                { typeof(ICouponRepository), typeof(CouponRepository) },
                { typeof(ICustomerRepository), typeof(CustomerRepository) },
                {typeof(ICustomerAddressRepository), typeof(CustomerAddressRepository) },   
                { typeof(IEventRepository), typeof(EventRepository) },
                { typeof(IPaymentTypeRepository), typeof(PaymentTypeRepository) },
                {typeof(IPrivacyPolicyPageRepository), typeof(PrivacyPolicyPageRepository)},
                { typeof(IQuestionRepository), typeof(QuestionRepository) },
                { typeof(IRoleManagementRepository), typeof(RoleManagementRepository) },
                {typeof(IRewardCouponCodeRepository), typeof(RewardCouponCodeRepository) },
                { typeof(IStateRepository), typeof(StateRepository) },
                {typeof(ITermsAndConditionRepository),typeof(TermsAndConditionRepository)},
                {typeof(ITestimonialReviewPageRepository),typeof(TestimonialReviewPageRepository)},
                { typeof(IUserRepository), typeof(UserRepository) }
            };
            return dataDictionary;
        }
    }
}
