using System;

namespace ServiceLocator {
    public class ServiceTable {
        private static readonly object l = new Object();
        private static ServiceCollection instance;

        public static ServiceCollection Services {
            get {
                lock (l) {
                    if (instance == null) {
                        instance = new ServiceCollection();
                    }
                }

                return instance;
            }
        }
    }
}
