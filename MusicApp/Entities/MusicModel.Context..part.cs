using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Entities
{
    public partial class MusicEntities
    {
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public override Task<int> SaveChangesAsync()
        {
            try
            {
                return base.SaveChangesAsync();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                return base.SaveChangesAsync(cancellationToken);
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public Song GetSong(string artist, string title)
        {
            Song song = this.Songs.FirstOrDefault(s => s.ArtistName == artist && s.Title == title);
            if (song == null)
            {
                song = new Song();
                song.ArtistName = artist;
                song.Title = title;
                this.Songs.Add(song);
            }

            return song;
        }

        public Video GetVideo(string sID)
        {
            Video video = this.Videos.FirstOrDefault(v => v.ID == sID);
            if (video == null)
            {
                video = new Video();
                video.ID = sID;
            }

            return video;
        }

        public Lyric GetLyric(string sText)
        {
            Lyric lyric = this.Lyrics.FirstOrDefault(l => l.Text == sText);
            if (lyric == null)
            {
                lyric = new Lyric();
                lyric.Text = sText;
            }

            return lyric;
        }

        public void Session(string key, string value)
        {
            Session session = this.Sessions.Where(s => s.Key == key).FirstOrDefault();
            if (session == null)
            {
                session = new Session();
                session.Key = key;
                this.Sessions.Add(session);
            }
            session.Value = value;
            this.SaveChanges();
        }

        public string Session(string key)
        {
            Session session = this.Sessions.Where(s => s.Key == key).FirstOrDefault();
            return session != null ? session.Value : null;
        }

        public void TraceClear(string key)
        {
            Session session = this.Sessions.Where(s => s.Key == key).FirstOrDefault();
            if (session != null)
            {
                session.Value = "";
                this.SaveChanges();
            }
        }

        public void Trace(string key, string value)
        {
            Session session = this.Sessions.Where(s => s.Key == key).FirstOrDefault();
            if (session == null)
            {
                session = new Session();
                session.Key = key;
                this.Sessions.Add(session);
                session.Value = value;
            }
            else
                session.Value += Environment.NewLine + value;
            this.SaveChanges();
        }
    }
}