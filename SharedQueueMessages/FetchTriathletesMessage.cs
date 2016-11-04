﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RaceAnalysis.SharedQueueMessages
{
    public class FetchTriathletesMessage
    {
        public int RaceId { get; set; }
        public int[] AgegroupIds { get; set; }
        public int[] GenderIds { get; set; }
    }
}