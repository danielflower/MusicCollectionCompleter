using NUnit.Framework;
using static MusicCollectionCompleter.Desktop.Services.Album;

namespace MusicCollectionCompleter.MusicCollectionCompleterTests.Services
{
    public class AlbumTest
    {

        [Test]
        public void ParenthesesAreRemovedFromAlbumTitles()
        {
            Assert.AreEqual("piscesiscariot", Normalise("Pisces Iscariot (Deluxe Edition)"));
            Assert.AreEqual("piscesiscariot", Normalise("The Pisces Iscariot (Deluxe Edition)"));
            Assert.AreEqual("piscesiscariot", Normalise("Pisces Iscariot"));
            Assert.AreEqual("piscesiscariot", Normalise("The Pisces Iscariot"));
            Assert.AreEqual("piscesiscariot", Normalise("Pisces Iscariot [Remastered]"));
            Assert.AreEqual("(Deluxe Edition)", Normalise("(Deluxe Edition)"));
        }

        [Test]
        public void PunctuationAintNoThing()
        {
            Assert.AreEqual("sirluciousleftfootthesonofchicodusty", Normalise("Sir Lucious Left Foot...The Son Of Chico Dusty"));
            Assert.AreEqual("sgtpepperslonelyheartsclubband", Normalise("Sgt. Pepper's Lonely Hearts Club Band"));
            Assert.AreEqual("doyoulikerockmusic", Normalise("Do You Like Rock Music?"));
            Assert.AreEqual("xandy", Normalise("X&Y"));
            Assert.AreEqual("eitheror", Normalise("Either/Or"));
            
        }
    }
}
