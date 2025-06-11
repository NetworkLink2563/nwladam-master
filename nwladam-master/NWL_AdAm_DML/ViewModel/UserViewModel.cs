namespace NWL_AdAm_DML.ViewModel
{
    public class UserListDataRowViewModel
    {
        public string userCode { get; set; } = string.Empty;

        public string userName { get; set; } = string.Empty;

        public string role { get; set; } = string.Empty;

        public string roleName
        {
            get
            {
                switch (role)
                {
                    case "1":
                        return "Viewer";
                    case "2":
                        return "Admin";
                    case "3":
                        return "Engineer";
                    default:
                        return role;
                };
            }
        }
    }

    public class UserInfoViewModel
    {
        public string userCode { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public string userName { get; set; } = string.Empty;
        public string customerCode { get; set; } = string.Empty;
        public string role { get; set; } = string.Empty;
    }


}
