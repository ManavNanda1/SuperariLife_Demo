using SuperariLife.Data.DBRepository.City;
using SuperariLife.Data.DBRepository.Country;
using SuperariLife.Data.DBRepository.Coupon;
using SuperariLife.Data.DBRepository.Customer;
using SuperariLife.Data.DBRepository.RoleManagement;
using SuperariLife.Data.DBRepository.User;
using SuperariLife.Service.Account;
using SuperariLife.Service.Appointment;
using SuperariLife.Service.Coupon.RewardCouponCode;
using SuperariLife.Service.Customer.CustomerAddress;
using SuperariLife.Service.Event;
using SuperariLife.Service.PaymentType;
using SuperariLife.Service.Question;
using SuperariLife.Service.SettingPage.AboutPage;
using SuperariLife.Service.SettingPage.PrivacyPolicyPage;
using SuperariLife.Service.SettingPage.TermsAndConditionPage;
using SuperariLife.Service.SettingPage.TestimonialReviewPage;
using SuperariLife.Service.State;

namespace SuperariLifeAPI.Service
{
    public static class ServiceRegister
    {
        public static Dictionary<Type, Type> GetTypes()
        {
            var serviceDictonary = new Dictionary<Type, Type>
            {
                {typeof(IAboutPageService),typeof(AboutPageService) },
                { typeof(IAccountService), typeof(AccountService) },
                { typeof(IAppointmentService), typeof(AppointmentService) },
                { typeof(ICityService), typeof(CityService) },
                { typeof(ICountryService), typeof(CountryService) },
                { typeof(ICouponService), typeof(CouponService) },
                { typeof(ICustomerService), typeof(CustomerService) },
                { typeof(ICustomerAddressService),typeof(CustomerAddressService) },
                { typeof(IEventService), typeof(EventService) },
                { typeof(IPaymentTypeService), typeof(PaymentTypeService) },
                {typeof(IPrivacyPolicyPageService),typeof (PrivacyPolicyPageService) },
                { typeof(IQuestionService), typeof(QuestionService) },
                { typeof(IRoleManagementService), typeof(RoleManagementService) },
                {typeof(IRewardCouponCodeService), typeof(RewardCouponCodeService) },   
                { typeof(IStateService), typeof(StateService) },
                {typeof(ITermsAndConditionPageService),typeof(TermsAndConditionPageService) },
                {typeof(ITestimonialReviewPageService),typeof(TestimonialReviewPageService) },  
                { typeof(IUserService), typeof(UserService) }
            };
            return serviceDictonary;
        }
    }
}
