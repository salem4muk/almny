namespace almny.Areas.Admin.Models.DTO
{
    public class CommentDto
    {
        public int CommentId { get; set; }
        public string CommentText { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public string UserId { get; set; }
        public string CommentDate { get; set; }
    }

}
