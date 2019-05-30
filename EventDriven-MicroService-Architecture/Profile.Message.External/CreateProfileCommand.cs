namespace Profile.Message.External
{
    public class CreateProfileCommand
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
