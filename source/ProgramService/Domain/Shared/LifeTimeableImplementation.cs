namespace Domain.Shared;

public sealed record LifeTimeableImplementation
{
    extension(ILifetimeable lifetimeable)
    {

        public void Update()
        {

            lifetimable.Lifetime = lifetimeable.Lifetime.Update();

        }

        public void Archive()
        {

            lifetimable.Lifetime = lifetimeable.Lifetime.Archive();

        }

        public void Restore()
        {

            lifetimable.Lifetime = lifetimeable.Lifetime.Restore();

        }

        public void ThrowIfNotActive()
        {

            if (lifetimeable.IsActive == false)
            {

                throw new InvalidOperationException("Запись находится в архиве");

            }

        }

    }
}
