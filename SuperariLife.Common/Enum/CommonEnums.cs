namespace SuperariLife.Common.Enum
{
    public static class AppointmentStatus
    {
        public const int Accepted = 1;
        public const int Pending = 0;
        public const int Rejected = -1;
       
    }
    public static class ActiveStatus
    {
        public const bool Active = true;
        public const bool Inactive = false;
    }
    public static class Status
    {
        public const int Failed = -1;
        public const int Success = 0;
        public const int InUse = -401;
        public const int URLExpired = -3;
        public const int URLUsed = -4;
        
    }
    public static class StatusResult
    {
        public const int AlreadyExists = -3;
        public const int Failed = -1;    
        public const int Updated = 0;
    }
    public static class LoginStatus
    {
        public const int CustomerDeactive = -2;
        public const int CustomerDeleted = -3;
        public const int EmailNotExist = -404;
        public const int UserDeactive = -2;
        public const int UserDeleted = -3;
      

    }  

  
}
