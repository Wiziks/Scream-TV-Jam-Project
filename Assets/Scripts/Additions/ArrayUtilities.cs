public static class ArrayUtilities {
    public static void Shuffle<T>(this T[] array) {
        int n = array.Length;
        for (int i = n - 1; i > 0; i--) {
            int j = UnityEngine.Random.Range(0, i + 1);

            T temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }

    public static void Shuffle<T>(this System.Collections.Generic.List<T> array) {
        int n = array.Count;
        for (int i = n - 1; i > 0; i--) {
            int j = UnityEngine.Random.Range(0, i + 1);

            T temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
}