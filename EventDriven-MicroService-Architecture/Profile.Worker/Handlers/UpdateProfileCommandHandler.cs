using System.Threading.Tasks;
using Profile.Message.External;
using Rebus.Bus;
using Rebus.Handlers;
using SharedKernel;

namespace Profile.Worker.Handlers
{
    public class UpdateProfileCommandHandler : IHandleMessages<UpdateProfileCommand>
    {
        private readonly IBus _bus;

        public UpdateProfileCommandHandler(IBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(UpdateProfileCommand message)
        {
            var profile = await DocumentDbRepository<Domain.Profile>
                .GetItemAsync(message.Id, message.Id, "Profiles");

            profile.FirstName = message.FirstName;
            profile.LastName = message.LastName;

            await DocumentDbRepository<Domain.Profile>
                .UpdateItemAsync(message.Id, "Profiles",  message.Id, profile);

            await _bus.Publish(new ProfileUpdatedEvent
            {
                Id = profile.id
            });
        }
    }
}