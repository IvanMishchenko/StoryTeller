namespace StoryTeller.Models
{
    public class Like
    {
        public int Id { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}