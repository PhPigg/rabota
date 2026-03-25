namespace Domain.Shared;

public sealed record LifeTimeableImplementation
{
    extension(ILifeTimeable lifetimeable)
    {

        public void Update()
        {

            lifetimeable.LifeTime = lifetimeable.LifeTime.Update();

        }

        public void Archive()
        {

            lifetimeable.LifeTime = lifetimeable.LifeTime.Archive();

        }

        public void Restore()
        {

            lifetimeable.LifeTime = lifetimeable.LifeTime.Restore();

        }

        public void ThrowIfNotActive()
        {

            if (lifetimeable.LifeTime.IsActive == false)
            {

                throw new InvalidOperationException("Запись находится в архиве");

            }

        }

    }
}
