using Microsoft.AspNetCore.Mvc;

namespace ImageShareWithPassword_Apr_17.Models
{
    public class ViewImageViewModel
    {
        public ImageSharing_Data.Image image;
        public bool hasPermissionToView;
        public int id;
    }
}
