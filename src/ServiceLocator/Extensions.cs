using System;

namespace ServiceLocator {
    static class Extensions {
        public static void MapStorageEndPoint(this ServiceCollection endPoints, Type endPointType, Type endPointImplementation) {
            endPoints.Add(endPointType, endPointImplementation);
        }
    }
}
