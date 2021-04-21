namespace FileManager.Helpers
{
    /// <summary>
    /// Сервис для конвертиции значений битов в выбранную размерность
    /// </summary>
    public static class ConverterService
    {
        /// <summary>
        /// Константа для перевода бит в гигабайты
        /// </summary>
        private const long BYTES_TO_GIGABYTES_DENOMINATOR = 1073741824;
        /// <summary>
        /// Константа для перевода бит в мегабайты
        /// </summary>
        private const long BYTES_TO_MEGABYTES_DENOMINATOR = 1048576;
        /// <summary>
        /// Метод для перевода бит в гигабайты
        /// </summary>
        /// <param name="bytes">Значение бит</param>
        /// <returns>Значение гигабайтов</returns>
        public static string BytesToGigabytesString(long bytes) => 
            string.Concat(
            ((double)bytes / BYTES_TO_GIGABYTES_DENOMINATOR).ToString("f2"),
            " GB");
        /// <summary>
        /// Метод для перевода бит в мегабайты
        /// </summary>
        /// <param name="bytes">Значение бит</param>
        /// <returns>Значение мегабайтов</returns>
        public static string BytesToMegabytesString(long bytes) =>
            string.Concat(
            ((double)bytes / BYTES_TO_MEGABYTES_DENOMINATOR).ToString("f2"),
            " MB");
    }
}
