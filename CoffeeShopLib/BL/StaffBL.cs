using Persistence;
using DAL;

namespace BL
{
    public class StaffBL
    {
        StaffDAL sDAL = new StaffDAL();
        public Staff? Authorize(string userName, string password)
        {
            Staff staff = sDAL.GetAccount(userName);
            if (staff.Password == sDAL.CreateMD5(password) && staff.StaffStatus == 0)
            {
                return staff;
            }
            else
            {
                return null;
            }
        }
    }
}
