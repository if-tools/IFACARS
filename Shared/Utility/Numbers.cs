namespace Shared.Utility
{
    public class Numbers
    {
        public static float MpsToKts(float mps)
        {
            return mps * 1.944f;
        }
        
        public static float MpsToFpm(float mps)
        {
            return mps * 197;
        }
    }
}