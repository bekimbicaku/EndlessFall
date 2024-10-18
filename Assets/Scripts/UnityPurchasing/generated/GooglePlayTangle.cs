// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("sckDikJUXGXchDiH/kd0qHDM6hE6wxQXJMgCeZPCSfjD/7dkWeJcjj92chgZaL78EB9ZFZCe1coKhqV2xdKSfQp22iegUKfhhExotV+0Qz1Zpwg4AlzfeO7RajERVXHj9+9ZLMvSwZ6Ems0bKQhUMVwbH+pW+BFtkRIcEyOREhkRkRISE7heSPFKJg1tNlkb1YmsgAROkviURcN0zbdG0COREjEjHhUaOZVbleQeEhISFhMQ+AiGtfdYQgjpe+nra/wyUviIttvwZU8ax0F1PLq18Q1H0KbFyWPH/gDV1Hq+hRp/zZrrxzf83/N3oCO6BpzQYVg1GzuznxbtBoAjqLTcL+oIOndgaV9M0fXUHIqoptxWDKvir+YKeUpHc7lzVhEQEhMS");
        private static int[] order = new int[] { 6,12,7,11,13,13,12,7,12,10,11,12,12,13,14 };
        private static int key = 19;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
