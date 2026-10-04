namespace Pokemon_Battle_Clone.Runtime.Stats.Domain
{
    public enum NatureEnum
    {
        Hardy, Lonely, Brave, Adamant, Naughty, Bold, Docile, Relaxed, Impish, Lax, Timid, Hasty, Serious, Jolly, Naive,
        Modest, Mild, Quiet, Bashful, Rash, Calm, Gentle, Sassy, Careful, Quirky
    }
    
    public struct Nature
    {
        public float Attack { get; private set; }
        public float Defense { get; private set; }
        public float SpcAttack { get; private set; }
        public float SpcDefense { get; private set; }
        public float Speed { get; private set; }

        private Nature(float attack, float spcAttack, float defense, float spcDefense, float speed)
        {
            Attack = attack;
            SpcAttack = spcAttack;
            Defense = defense;
            SpcDefense = spcDefense;
            Speed = speed;
        }
        
        public static Nature FromEnum(NatureEnum natureEnum) =>
            natureEnum switch
            {
                NatureEnum.Adamant => Adamant(),
                NatureEnum.Bashful => Bashful(),
                NatureEnum.Bold => Bold(),
                NatureEnum.Brave => Brave(),
                NatureEnum.Calm => Calm(),
                NatureEnum.Careful => Careful(),
                NatureEnum.Docile => Docile(),
                NatureEnum.Gentle => Gentle(),
                NatureEnum.Hardy => Hardy(),
                NatureEnum.Hasty => Hasty(),
                NatureEnum.Impish => Impish(),
                NatureEnum.Jolly => Jolly(),
                NatureEnum.Lax => Lax(),
                NatureEnum.Lonely => Lonely(),
                NatureEnum.Mild => Mild(),
                NatureEnum.Modest => Modest(),
                NatureEnum.Naive => Naive(),
                NatureEnum.Naughty => Naughty(),
                NatureEnum.Quiet => Quiet(),
                NatureEnum.Quirky => Quirky(),
                NatureEnum.Rash => Rash(),
                NatureEnum.Relaxed => Relaxed(),
                NatureEnum.Sassy => Sassy(),
                NatureEnum.Serious => Serious(),
                NatureEnum.Timid => Timid(),
                _ => Bashful()
            };

        /// <summary>
        /// Neutral
        /// </summary>
        public static Nature Hardy() => new Nature(attack: 1f, spcAttack: 1f, defense: 1f, spcDefense: 1f, speed: 1f);
        /// <summary>
        /// Increased stat: Attack<br/>
        /// Decreased stat: Defense
        /// </summary>
        public static Nature Lonely() => new Nature(attack: 1.1f, spcAttack: 1f, defense: 0.9f, spcDefense: 1f, speed: 1f);
        /// <summary>
        /// Increased stat: Attack<br/>
        /// Decreased stat: Speed
        /// </summary>
        public static Nature Brave() => new Nature(attack: 1.1f, spcAttack: 1f, defense: 1f, spcDefense: 1f, speed: 0.9f);
        /// <summary>
        /// Increased stat: Attack<br/>
        /// Decreased stat: Sp. Attack
        /// </summary>
        public static Nature Adamant() => new Nature(attack: 1.1f, spcAttack: 0.9f, defense: 1f, spcDefense: 1f, speed: 1f);
        /// <summary>
        /// Increased stat: Attack<br/>
        /// Decreased stat: Sp. Defense
        /// </summary>
        public static Nature Naughty() => new Nature(attack: 1.1f, spcAttack: 1f, defense: 1f, spcDefense: 0.9f, speed: 1f);
        /// <summary>
        /// Increased stat: Defense<br/>
        /// Decreased stat: Attack
        /// </summary>
        public static Nature Bold() => new Nature(attack: 0.9f, spcAttack: 1f, defense: 1.1f, spcDefense: 1f, speed: 1f);
        /// <summary>
        /// Neutral
        /// </summary>
        public static Nature Docile() => new Nature(attack: 1f, spcAttack: 1f, defense: 1f, spcDefense: 1f, speed: 1f);
        /// <summary>
        /// Increased stat: Defense<br/>
        /// Decreased stat: Speed
        /// </summary>
        public static Nature Relaxed() => new Nature(attack: 1f, spcAttack: 1f, defense: 1.1f, spcDefense: 1f, speed: 0.9f);
        /// <summary>
        /// Increased stat: Defense<br/>
        /// Decreased stat: Sp. Attack
        /// </summary>
        public static Nature Impish() => new Nature(attack: 1f, spcAttack: 0.9f, defense: 1.1f, spcDefense: 1f, speed: 1f);
        /// <summary>
        /// Increased stat: Defense<br/>
        /// Decreased stat: Sp. Defense
        /// </summary>
        public static Nature Lax() => new Nature(attack: 1f, spcAttack: 1f, defense: 1.1f, spcDefense: 0.9f, speed: 1f);
        /// <summary>
        /// Increased stat: Speed<br/>
        /// Decreased stat: Attack
        /// </summary>
        public static Nature Timid() => new Nature(attack: 0.9f, spcAttack: 1f, defense: 1f, spcDefense: 1f, speed: 1.1f);
        /// <summary>
        /// Increased stat: Speed<br/>
        /// Decreased stat: Defense
        /// </summary>
        public static Nature Hasty() => new Nature(attack: 1f, spcAttack: 1f, defense: 0.9f, spcDefense: 1f, speed: 1.1f);
        /// <summary>
        /// Neutral
        /// </summary>
        public static Nature Serious() => new Nature(attack: 1f, spcAttack: 1f, defense: 1f, spcDefense: 1f, speed: 1f);
        /// <summary>
        /// Increased stat: Speed<br/>
        /// Decreased stat: Sp. Attack
        /// </summary>
        public static Nature Jolly() => new Nature(attack: 1f, spcAttack: 0.9f, defense: 1f, spcDefense: 1f, speed: 1.1f);
        /// <summary>
        /// Increased stat: Speed<br/>
        /// Decreased stat: Sp. Defense
        /// </summary>
        public static Nature Naive() => new Nature(attack: 1f, spcAttack: 1f, defense: 0.9f, spcDefense: 1f, speed: 1.1f);
        /// <summary>
        /// Increased stat: Sp. Attack<br/>
        /// Decreased stat: Attack
        /// </summary>
        public static Nature Modest() => new Nature(attack: 0.9f, spcAttack: 1.1f, defense: 1f, spcDefense: 1f, speed: 1f);
        /// <summary>
        /// Increased stat: Sp. Attack<br/>
        /// Decreased stat: Defense
        /// </summary>
        public static Nature Mild() => new Nature(attack: 1f, spcAttack: 1.1f, defense: 0.9f, spcDefense: 1f, speed: 1f);
        /// <summary>
        /// Increased stat: Sp. Attack<br/>
        /// Decreased stat: Speed
        /// </summary>
        public static Nature Quiet() => new Nature(attack: 1f, spcAttack: 1.1f, defense: 1f, spcDefense: 1f, speed: 0.9f);
        /// <summary>
        /// Neutral
        /// </summary>
        public static Nature Bashful() => new Nature(attack: 1f, spcAttack: 1f, defense: 1f, spcDefense: 1f, speed: 1f);
        /// <summary>
        /// Increased stat: Sp. Attack<br/>
        /// Decreased stat: Sp. Defense
        /// </summary>
        public static Nature Rash() => new Nature(attack: 1f, spcAttack: 1.1f, defense: 1f, spcDefense: 0.9f, speed: 1f);
        /// <summary>
        /// Increased stat: Sp. Defense<br/>
        /// Decreased stat: Attack
        /// </summary>
        public static Nature Calm() => new Nature(attack: 0.9f, spcAttack: 1f, defense: 1f, spcDefense: 1.1f, speed: 1f);
        /// <summary>
        /// Increased stat: Sp. Defense<br/>
        /// Decreased stat: Defense
        /// </summary>
        public static Nature Gentle() => new Nature(attack: 1f, spcAttack: 1f, defense: 0.9f, spcDefense: 1.1f, speed: 1f);
        /// <summary>
        /// Increased stat: Sp. Defense<br/>
        /// Decreased stat: Speed
        /// </summary>
        public static Nature Sassy() => new Nature(attack: 1f, spcAttack: 1f, defense: 1f, spcDefense: 1.1f, speed: 0.9f);
        /// <summary>
        /// Increased stat: Sp. Defense<br/>
        /// Decreased stat: Sp. Attack
        /// </summary>
        public static Nature Careful() => new Nature(attack: 1f, spcAttack: 0.9f, defense: 1f, spcDefense: 1.1f, speed: 1f);
        /// <summary>
        /// Neutral
        /// </summary>
        public static Nature Quirky() => new Nature(attack: 1f, spcAttack: 1f, defense: 1f, spcDefense: 1f, speed: 1f);
    }
}