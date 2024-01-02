using System.Security.Cryptography;

namespace SuperariLife.Common.Helpers
{
    public class StoredProcedures
    {
        #region Admin Portal
        #region Appointment
        public const string DeleteAppointmentByCustomer = "SP_DeleteAppointmentByCustomer";
        public const string GetAppointmentDetailByIdForAdmin = "SP_GetAppointmentDetailByIdForAdmin";
        public const string GetAppointmentDetailByIdForCustomer = "SP_GetAppointmentDetailByIdForCustomer";
        public const string GetAppointmentTypeForDropDown = "SP_GetAppointmentTypeForDropDown";
        public const string GetAppointmentListByAdmin = "SP_GetAppointmentListByAdmin";
        public const string GetAppointmentListByCustomer = "SP_GetAppointmentListByCustomer";
        public const string InsertUpdateAppointmentByAdmin = "SP_InsertUpdateAppointmentByAdmin";
        public const string InsertUpdateAppointmentByCustomer = "SP_InsertUpdateAppointmentByCustomer";
        #endregion

        #region City
        public const string DeleteCity = "SP_DeleteCity";
        public const string GetCityByStateId = "SP_GetCityByStateId";
        public const string GetCityById = "SP_GetCityById";
        public const string InsertUpdateCity = "SP_InsertUpdateCity";
        #endregion

        #region Country
        public const string DeleteCountry = "SP_DeleteCountry";
        public const string GetCountry = "SP_GetCountry";
        public const string GetCountryById = "SP_GetCountryById";
        public const string InsertUpdateCountry = "SP_InsertUpdateCountry";
        #endregion

        #region CouponCode
        public const string DeleteCouponCode = "SP_DeleteCouponCode";
        public const string GetCouponListByAdmin = "SP_GetCouponListByAdmin";
        public const string GetCouponCodeById = "SP_GetCouponCodeById";
        public const string GetCouponCodeDetailById = "SP_GetCouponCodeDetailById";
        public const string InsertUpdateCouponCode = "SP_InsertUpdateCouponCode";
        #endregion

        #region Customer
        public const string ActiveDeactiveCustomerByAdmin = "SP_ActiveDeactiveCustomerByAdmin";
        public const string DeleteCustomerByAdmin = "SP_DeleteCustomerByAdmin";
        public const string GetCustomerListByAdmin = "SP_GetCustomerListByAdmin";
        public const string GetCustomerByIdByAdmin = "SP_GetCustomerByIdByAdmin";
        public const string InsertUpdateCustomerByAdmin = "SP_InsertUpdateCustomerByAdmin";
        #endregion

        #region  Event
        public const string DeleteEventGalleryImage = "SP_DeleteEventGalleryImage";
        public const string DeleteEventQuestion = "SP_DeleteEventQuestion";
        public const string DeleteEvent = "SP_DeleteEvent";
        public const string GetEventListByAdmin = "SP_GetEventListByAdmin";
        public const string GetEventByIdForAdmin = "SP_GetEventByIdForAdmin";
        public const string GetEventDetailOfCustomerParticipant = "SP_GetEventDetailOfCustomerParticipant";
        public const string GetEventDetailOfQuestion = "SP_GetEventDetailOfQuestion";
        public const string InsertUpdateEvent = "SP_InsertUpdateEvent";
        #endregion

        #region Login
        public const string CustomerLogin = "SP_CustomerLogin";
        public const string ForgetPasswordForUser = "SP_GetUserIDByEmail";
        public const string ForgetPasswordForCustomer = "SP_GetCustomerIDByEmail";
        public const string ForgetPasswordChangeWithURL = "SP_ForgetPasswordChangeWithURL";
        public const string ForgetPasswordChangeWithURLForCustomer = "SP_ForgetPasswordChangeWithURLForCustomer";
        public const string GetUserSaltByEmail = "SP_User_GetSaltByEmail";
        public const string GetCustomerSaltByEmail = "SP_Customer_GetSaltByEmail";
        public const string GetUserIdByEmail = "SP_GetUserIDByEmail";
        public const string LoginUser = "SP_UserLogin";
        public const string LogoutUser = "SP_LogoutUser";
        public const string LogoutCustomer = "SP_LogoutCustomer";
        public const string ResetPassword = "SP_User_UpdatePassword";
        public const string SaveOTP = "SP_EmailOTP_Add";
        public const string UpdateLoginToken = "SP_UpdateLoginToken";
        public const string UpdateLoginTokenForCustomer = "SP_UpdateLoginTokenForCustomer";
        public const string ValidateToken = "SP_ValidateToken";
        public const string VerifyOTP = "sp_EmailOTP_Verify";
        #endregion

        #region PaymentType
        public const string DeletePaymentType = "SP_DeletePaymentType";
        public const string GetPaymentTypeList = "SP_GetPaymentTypeList";
        public const string GetPaymentTypeById = "SP_GetPaymentTypeById ";
        public const string InsertUpdatePaymentType = "SP_InsertUpdatePaymentType";
        #endregion

        #region  Pages(Settings)
        //About Page
        #region About Section 
            public const string DeleteAboutPageSection = "SP_DeleteAboutPageSection";
            public const string GetAboutPageSectionLayoutTypeForDropDown = "SP_GetAboutPageSectionLayoutTypeForDropDown";
            public const string GetAboutPageSectionList= "SP_GetAboutPageSectionList";
            public const string GetAboutPageSectionByCustomer = "SP_GetAboutPageSectionByCustomer";
            public const string GetAboutPageSectionById = "SP_GetAboutPageSectionById";
            public const string InsertUpdateAboutPageSection= "SP_InsertUpdateAboutPageSection";
            public const string RemoveSectionAboutImage = "Sp_RemoveSectionAboutImage";
        #endregion

        #region About  Image 
        public const string DeleteAboutPageImage= "SP_DeleteAboutPageImage";
        public const string GetAboutPageImageList= "SP_GetAboutPageImageList";
        public const string GetAboutPageImageById= "SP_GetAboutPageImageById";
        public const string InsertUpdateAboutPageImage= "SP_InsertUpdateAboutPageImage";
            #endregion


        //Privacy Page
        public const string GetPrivacyPage = "SP_GetPrivacyPage";
        public const string InsertUpdatePrivacyPage = "SP_InsertUpdatePrivacyPage";

        //Terms and Condition Page
        public const string GetTermsAndConditionPage = "SP_GetTermsAndConditionPage";
        public const string InsertUpdateTermsAndConditionPage= "SP_InsertUpdateTermsAndConditionPage";

        //Testimonial Page
        public const string DeleteTestimonialPagesReview= "SP_DeleteTestimonialPagesReview";
        public const string GetTestimonialPagesReviewList = "SP_GetTestimonialPagesReviewList";
        public const string GetTestimonialPagesReviewForCustomer = "SP_GetTestimonialPagesReviewForCustomer";
        public const string GetTestimonialPagesReviewById = "SP_GetTestimonialPagesReviewById";
        public const string InsertUpdateTestimonialPagesReview = "SP_InsertUpdateTestimonialPagesReview";
        #endregion

        #region Question
        public const string DeleteQuestion = "SP_DeleteQuestion";
        public const string GetQuestionById = "SP_GetQuestionById";
        public const string GetQuestionListByAdmin = "SP_GetQuestionListByAdmin";
        public const string GetQuestionTypeListByAdmin = "SP_GetQuestionTypeListByAdmin";
        public const string GetQuestionListForDropDownList = "SP_GetQuestionListForDropDownList";
        public const string InsertUpdateQuestion = "SP_InsertUpdateQuestion";
        #endregion

        #region RoleManagement
        public const string DeleteRoleManagement = "SP_DeleteRoleManagement";
        public const string GetRoleManagement = "SP_GetRoleManagement";
        public const string GetRoleManagementById = "SP_GetRoleManagementById";
        public const string GetRoleListForDropDown = "SP_GetRoleListForDropDown";
        public const string InsertUpdateRoleManagement = "SP_InsertUpdateRoleManagement";
        #endregion

        #region Rewards Coupon Code
        public const string GetRewardCouponCodeListByAdmin = "SP_GetRewardCouponCodeListByAdmin";
        public const string GetCustomerListForSendingCouponCodeReward = "SP_GetCustomerListForSendingCouponCodeReward";
        public const string InsertUpdateRewardCouponCode = "SP_InsertUpdateRewardCouponCode";
        #endregion

        #region State
        public const string DeleteState = "SP_DeleteState";
        public const string GetStateByCountryId = "SP_GetStateByCountryId";
        public const string GetStateById = "SP_GetStateById";
        public const string InsertUpdateState = "SP_InsertUpdateState";
        #endregion

        #region User
        public const string ChangePassword = "SP_UpdatePasswordByUser";
        public const string ChangePasswordForCustomer = "SP_UpdatePasswordByCustomer";
        public const string GetUserById = "SP_GetUserById";
        public const string InsertUpdateUserByUser = "SP_InsertUpdateUserByUser";


        #endregion

        #region  UserAdmin
        public const string ActiveDeactiveUserByAdmin = "SP_ActiveDeactiveUserByAdmin";
        public const string DeleteUserByAdmin = "SP_DeleteUserByAdmin";
        public const string GetUserByIdByAdmin = "SP_GetUserByIdByAdmin";
        public const string GetUserListByAdmin = "SP_GetUserListByAdmin";
        public const string GetUserListForDropDownList = "SP_GetUserListForDropDownList";
        public const string InsertUpdateUserByAdmin = "SP_InsertUpdateUserByAdmin";
        #endregion

        #endregion


        #region Customer Portal

        #region Customer
        public const string InsertUpdateCustomer = "SP_InsertUpdateCustomer";
        #endregion

        #region Customer Address
        public const string DeleteCustomerAddress= "SP_DeleteCustomerAddress";
        public const string GetCustomerAddressById = "SP_GetCustomerAddressById";
        public const string GetCustomerAddressList= "SP_GetCustomerAddressList";
        public const string InsertUpdateCustomerAddressByCustomer= "SP_InsertUpdateCustomerAddressByCustomer";
        #endregion

        #endregion

    }
}
