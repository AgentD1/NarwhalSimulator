﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICreature : IDamageable {
	string creatureType { get; }
}
