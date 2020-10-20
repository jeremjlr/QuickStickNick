using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using System.Drawing.Imaging;


namespace QuickStickNick
{
    #region Classes and auras enums
    /// <summary>
    /// All the available classes
    /// </summary>
    public enum Classes
    {
        BM,
        Survival,
        Arms,
        Prot
    }

    /// <summary>
    /// List of the monitored auras, used as an ID for each aura
    /// </summary>
    public enum AuraNames
    {
        //Bm
        KillCommand,
        KillCommandBurst,
        BarbedShot,
        BeastCleave,
        BeastCleave3,
        BeastCleave4,
        Focus,
        //Survival
        Focus87,
        SerpentSting,
        Wildfire,
        NotMongooseBite,
        NotSerpentStingBomb,
        //Prot
        Avatar,
        AvatarNotUp,
        DemoShout,
        ShieldSlam,
        ThunderClap,
        IgnorePain,
        NotFreeRevenge,
        RageLowerThan40,
        NotVictoryRush,
        //Arms
        Skullsplitter,
        NotInForTheKill,
        Overpower,
        MortalStrike,
        NotWW,
        NotExec,
        SweepingStrikes,
        NotVictoryRushArms
    }
    #endregion
    public class Aura
    {
        #region CONST
        //Path where all the calibrated pictures (image of the auras when it's present) will be stored
        private const string CALIBRATION_PATH = "Calibrations/";
        #endregion

        #region Private attributes
        //Current image of what is at the aura's position
        private Image<Bgr, byte> aura;
        //Aura position on the screen
        private int xPos;
        private int yPos;
        private int width;
        private int height;
        #endregion

        #region Public attributes

        /// <summary>
        /// Name and ID of the aura
        /// </summary>
        public AuraNames Name
        {
            get;
            private set;
        }
        /// <summary>
        /// The class the aura belongs to.
        /// </summary>
        public Classes GameClass
        {
            get;
            private set;
        }
        /// <summary>
        /// Returns true if the aura is currently present on the screen and detected.
        /// </summary>
        public bool Present
        {
            get;
            private set;
        }
        #endregion

        #region Constructor and public methods
        public Aura(AuraNames name, Classes gameClass, int xPos, int yPos, int width, int height)
        {
            Name = name;
            GameClass = gameClass;
            this.xPos = xPos;
            this.yPos = yPos;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Calibrates the aura with what is on the screen at the aura's position when the method is called.
        /// </summary>
        public void Calibrate()
        {
            aura = GetAura();
            aura.Save(CALIBRATION_PATH + Name.ToString() + ".bmp");
        }

        /// <summary>
        /// Refreshes the aura's status.
        /// </summary>
        public void Update()
        {
            //Gets the aura on the screen and compares it to the one stored during calibration
            Present = IsSimilar(aura, GetAura());
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Returns true if both images are similar.
        /// </summary>
        /// <param name="aura1"></param>
        /// <param name="aura2"></param>
        /// <returns></returns>
        private bool IsSimilar(Image<Bgr, byte> aura1, Image<Bgr, byte> aura2)
        {
            Bgr averageDiff = aura1.AbsDiff(aura2).GetAverage();
            if (averageDiff.Red < 10 && averageDiff.Blue < 10 && averageDiff.Green < 10)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns an image of what is on the screen at the aura's position.
        /// </summary>
        /// <param name="xpos"></param>
        /// <param name="ypos"></param>
        /// <returns></returns>
        private Image<Bgr, byte> GetAura()
        {
            Bitmap aura = new Bitmap(
                        width,
                        height,
                        PixelFormat.Format32bppArgb);

            Graphics g = Graphics.FromImage(aura);
            g.CopyFromScreen(
                xPos,
                yPos,
                0,
                0,
                new Size(width, height),
                CopyPixelOperation.SourceCopy);

            return new Image<Bgr, byte>(aura);
        }
        #endregion
    }
}
