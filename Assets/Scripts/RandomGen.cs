public class RandomGen
{
    uint[] state = new uint[4];

    public RandomGen(int seed)
    {
        state[0] = (uint)seed;
        state[1] = (uint)seed + 1;
        state[2] = (uint)seed + 2;
        state[3] = (uint)seed + 4;
    }

    public RandomGen(string seedStr)
        : this(seedStr.GetHashCode())
    { }

    public RandomGen(RandomGen other)
    {
        state[0] = other.state[0];
        state[1] = other.state[1];
        state[2] = other.state[2];
        state[3] = other.state[3];
    }

    public int NextInt()
    {
        return (int)(next() & 0x7FFFFFFF);
    }

    public int NextInt(int max)
    {
        return NextInt() % max;
    }

    public int NextInt(int min, int max)
    {
        return (NextInt() % max) + min;
    }

    public float NextFloat()
    {
        return (float)NextInt(0, 0x1000000) / 0x1000000;
    }

    public float NextFloat(float max)
    {
        return NextFloat() * max;
    }

    public float NextFloat(float min, float max)
    {
        return NextFloat(max - min) + min;
    }

    private uint next()
    {
        uint t = state[3];
        t ^= t << 11;
        t ^= t >> 8;
        state[3] = state[2];
        state[2] = state[1];
        state[1] = state[0];
        t ^= state[0];
        t ^= state[0] >> 15;
        state[0] = t;
        return t;
    }
}