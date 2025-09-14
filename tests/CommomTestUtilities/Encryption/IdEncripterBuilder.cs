using Sqids;

namespace CommomTestUtilities.Encryption;

public class IdEncripterBuilder
{
    public static SqidsEncoder<long> Build()
    {
        return new SqidsEncoder<long>(new SqidsOptions() { MinLength = 3 });
    }
}
