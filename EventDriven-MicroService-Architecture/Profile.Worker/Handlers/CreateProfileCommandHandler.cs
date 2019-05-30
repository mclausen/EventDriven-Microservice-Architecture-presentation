using System;
using System.Threading.Tasks;
using Profile.Message.External;
using Rebus.Bus;
using Rebus.Handlers;
using SharedKernel;

namespace Profile.Worker.Handlers
{
    public class CreateProfileCommandHandler : IHandleMessages<CreateProfileCommand>
    {
        private readonly IBus _bus;

        public CreateProfileCommandHandler(IBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(CreateProfileCommand message)
        {
            var profile = new Domain.Profile()
            {
                id = message.UserId,
                FirstName = message.FirstName,
                LastName = message.LastName
            };

            await DocumentDbRepository<Domain.Profile>
                .CreateItemAsync(profile, profile.id, "Profiles");

            await _bus.Publish(new ProfileCreatedEvent()
            {
                Id = profile.id
            });
        }
    }
}
