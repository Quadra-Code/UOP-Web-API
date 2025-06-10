using System;
using System.Security.Cryptography;

namespace UOP.Domain.Extensions
{
    public static class UuidV7
    {
        public static Guid NewGuid()
        {
            byte[] guidBytes = new byte[16];
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            // Inject timestamp (48 bits)
            Buffer.BlockCopy(BitConverter.GetBytes(timestamp), 0, guidBytes, 0, 6);

            // Fill the rest with random bytes
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(guidBytes, 6, 10);
            }
            // Set the version (7)
            guidBytes[6] = (byte)((guidBytes[6] & 0x0F) | (7 << 4));

            // Set the variant (RFC 4122)
            guidBytes[8] = (byte)((guidBytes[8] & 0x3F) | 0x80);

            return new Guid(guidBytes);
        }
    }
}
