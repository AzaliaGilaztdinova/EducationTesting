using System;

namespace EducationTesting.Client
{
    public class GuidProvider : IGuidProvider
    {
        public string NewGuid() => Guid.NewGuid().ToString();
    }
}