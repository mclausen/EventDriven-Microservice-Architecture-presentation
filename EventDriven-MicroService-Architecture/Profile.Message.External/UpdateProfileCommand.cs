namespace Profile.Message.External
{
    public class UpdateProfileCommand
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}