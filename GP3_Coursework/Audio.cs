using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace GP3_Coursework
{
    class Audio
    {

        Dictionary<string, Song> dc_SongCollection = new Dictionary<string, Song>();
        Dictionary<string, SoundEffect> dc_SoundFxCollection = new Dictionary<string,SoundEffect>();
        public bool isPlaying = true;

        public void addSong(string songName, Song songObject)
        {
         
            dc_SongCollection.Add(songName, songObject);
        }

        public void getSong(string songName)
        {

            Song tempSong;
            if(dc_SongCollection.ContainsKey(songName))
                {
                    tempSong = dc_SongCollection[songName];
                }
        
        }

        public void AddSoundEffect(string effectName,SoundEffect effectObject)
        {
            dc_SoundFxCollection.Add(effectName,effectObject);
        }

  
        /// <summary>
        /// Play the given song
        /// </summary>
        /// <param name="songName"> The name of the song to play</param>
        public void PlaySong(string songName)       
        {
            if (dc_SongCollection.ContainsKey(songName))
            {
                Song songToPlay = dc_SongCollection[songName];
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(songToPlay);  
            }
            else
                return;
                
        }

        public void stopSong()
        {
            MediaPlayer.Pause();
        }

        public void startSong()
        {
            MediaPlayer.Resume();
        }

        

        public void PlaySoundEffect(string fxName)        
        {
            if(dc_SoundFxCollection.ContainsKey(fxName))
            {
                SoundEffect.MasterVolume = 1f;
                SoundEffect temp = dc_SoundFxCollection[fxName];
                temp.Play();    
            }
        }

        public void StopSoundEffect(string fxName)
        {
            if (dc_SoundFxCollection.ContainsKey(fxName))
            {
                SoundEffect temp = dc_SoundFxCollection[fxName];
                SoundEffect.MasterVolume=0f;
                
            }
        }
        public void DelSound(string fxName)
        {
            if (dc_SoundFxCollection.ContainsKey(fxName))
            {
                dc_SoundFxCollection.Remove(fxName);
            }
        }

    }
}
