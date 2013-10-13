namespace SmaugCS.Common
{
    public static class Check
    {
        /// <summary>
        /// Returns the lower of two numbers
        /// </summary>
        /// <param name="check"></param>
        /// <param name="ncheck"></param>
        /// <returns></returns>
        public static int Minimum(int check, int ncheck)
        {
            return check < ncheck ? check : ncheck;
        }

        /// <summary>
        /// Returns the higher of two numbers
        /// </summary>
        /// <param name="check"></param>
        /// <param name="ncheck"></param>
        /// <returns></returns>
        public static int Maximum(int check, int ncheck)
        {
            return check > ncheck ? check : ncheck;
        }

        /// <summary>
        /// Validates a number between a range or returns the range values
        /// </summary>
        /// <param name="mincheck"></param>
        /// <param name="check"></param>
        /// <param name="maxcheck"></param>
        /// <returns></returns>
        public static int Range(int mincheck, int check, int maxcheck)
        {
            if (check < mincheck) return mincheck;
            return check > maxcheck ? maxcheck : check;
        }
    }
}
