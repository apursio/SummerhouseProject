using UnityEngine;

public class FireAnimationController : MonoBehaviour
{
    public ParticleSystem heatDistortion;
    public ParticleSystem flames;
    public ParticleSystem flamesSecondary;
    public ParticleSystem lights;
    public Light pointLightFire;

    private float timer = 0f;

    void Start()
    {
        // Set initial emission rates to 0
        SetEmissionRate(heatDistortion, 0f);
        SetEmissionRate(flames, 0f);
        SetEmissionRate(flamesSecondary, 0f);
        SetEmissionRate(lights, 0f);

        // Set initial intensity to 0
        SetPointLightIntensity(0f);

        // Set initial start size to 0 for "Flames"
        SetStartSize(flames, 0f, 0f);

        // Set initial velocity over lifetime for "Flames"
        SetVelocityOverLifetime(flames, new Vector3(0f, 0f, 0f));

        // Set initial speed modifier for "Flames"
        SetSpeedModifier(flames, 1f);
    }

    void Update()
    {
        timer += Time.deltaTime;

        // From 0 to 10 seconds, grow "Heat Distortion" emission rate to 15
        if (timer <= 10f)
        {
            SetEmissionRate(heatDistortion, Mathf.Lerp(0f, 15f, timer / 10f));
        }

        // From 10 to 20 seconds, adjust "Flames" parameters
        if (timer > 10f && timer <= 20f)
        {
            // Adjust "Flames" start size to values between 0.01 and 0.02
            SetStartSize(flames, Mathf.Lerp(0.01f, 0.02f, (timer - 10f) / 10f), Mathf.Lerp(0.03f, 0.04f, (timer - 10f) / 10f));

            // Adjust "Flames" velocity over lifetime for "Linear Y" to values between 0.4 and 0.1
            SetVelocityOverLifetime(flames, new Vector3(0f, Mathf.Lerp(0.1f, 0.6f, (timer - 10f) / 10f), 0f));

            // Set speed modifier to 1
            SetSpeedModifier(flames, 1f);

            // Emission rate over time 0-5
            SetEmissionRate(flames, Mathf.Lerp(0f, 5f, (timer - 10f) / 10f));
        }

        // From 20 to 50 seconds, adjust "Flames" parameters
        if (timer > 20f && timer <= 50f)
        {
            // Adjust "Flames" start size to values between 0.01 and 0.02
            SetStartSize(flames, Mathf.Lerp(0.04f, 0.1f, (timer - 20f) / 30f), Mathf.Lerp(0.04f, 1.4f, (timer - 20f) / 30f));

            // Adjust "Flames" velocity over lifetime for "Linear Y" to smoothly transition from 0.4 to 0.1
            SetVelocityOverLifetime(flames, new Vector3(0f, Mathf.Lerp(0.4f, 1.1f, (timer - 20f) / 30f), 0f));


            // Set speed modifier
          //  SetSpeedModifier(flames, Mathf.Lerp(0.6f, 1.2f, (timer - 20f) / 30f));

            // Emission rate over time 5-15
            SetEmissionRate(flames, Mathf.Lerp(5f, 15f, (timer - 20f) / 30f));
        }

        // From 30 to 50 seconds, grow "Flames Secondary" emission rate to 15
        if (timer > 30f && timer <= 50f)
        {
            SetEmissionRate(flamesSecondary, Mathf.Lerp(0f, 15f, (timer - 30f) / 20f));
            // Set speed modifier to linearly change from 2 to 1 over time 10-20 seconds
            SetSpeedModifier(flamesSecondary, Mathf.Lerp(0.4f, 1f, (timer - 30f) / 20f));
        }

        // From 25 to 50 seconds, grow "Lights" emission rate to 5
        if (timer > 25f && timer <= 50f)
        {
            SetEmissionRate(lights, Mathf.Lerp(0f, 5f, (timer - 25f) / 25f));
        }

        // From 25 to 50 seconds, grow "Point LightFire" intensity from 0 to 1
        if (timer > 25f && timer <= 50f)
        {
            SetPointLightIntensity(Mathf.Lerp(0f, 1f, (timer - 25f) / 25f));
        }
    }

    // Helper method to set the emission rate of a Particle System
    void SetEmissionRate(ParticleSystem particleSystem, float rate)
    {
        var emissionModule = particleSystem.emission;
        emissionModule.rateOverTime = rate;
    }

    // Helper method to set the start size of a Particle System
    void SetStartSize(ParticleSystem particleSystem, float startSizeMin, float startSizeMax)
    {
        var mainModule = particleSystem.main;
        mainModule.startSizeX = new ParticleSystem.MinMaxCurve(startSizeMin, startSizeMax);
    }

    // Helper method to set the velocity over lifetime of a Particle System
    void SetVelocityOverLifetime(ParticleSystem particleSystem, Vector3 velocity)
    {
        var velocityOverLifetimeModule = particleSystem.velocityOverLifetime;
        velocityOverLifetimeModule.enabled = true;
        velocityOverLifetimeModule.space = ParticleSystemSimulationSpace.World;
        velocityOverLifetimeModule.x = new ParticleSystem.MinMaxCurve(0f, 0f); // Keep Linear X at 0
        velocityOverLifetimeModule.y = new ParticleSystem.MinMaxCurve(velocity.y, velocity.y);
        velocityOverLifetimeModule.z = new ParticleSystem.MinMaxCurve(0f, 0f); // Keep Linear Z at 0
    }

    // Helper method to set the intensity of a Point Light
    void SetPointLightIntensity(float intensity)
    {
        pointLightFire.intensity = intensity;
    }

    // Helper method to set the speed modifier of a Particle System
    void SetSpeedModifier(ParticleSystem particleSystem, float speedModifier)
    {
        var mainModule = particleSystem.main;
        mainModule.simulationSpeed = speedModifier;
    }
}
