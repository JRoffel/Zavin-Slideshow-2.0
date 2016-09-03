using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Zavin.Slideshow.wpf
{
    [Table(Name = "mcmain.wachtboek")] //Unsure about real table name, so this is just a placeholder... Might be correct tho :P
    class ProductionDatabaseModel
    {
        private int _EntryId; //Will change these names to match with columns in the database, but have no remote access :/
        [Column(IsPrimaryKey = true, Storage = "_EntryId")] //Not sure what actually goes in the "Storage" part, just guessing now, will test with DB access :P
        public int EntryId
        {
            get
            {
                return _EntryId;
            }

            set
            {
                _EntryId = value;
            }
        }

        private int _wb_verstoo;
        [Column(IsPrimaryKey = false, Storage = "_wb_verstoo")] //Pretty sure these names will match, needs check tho...
        public int wb_verstoo
        {
            get
            {
                return _wb_verstoo;
            }

            set
            {
                _wb_verstoo = value;
            }
        }

        private int _wb_wasta;
        [Column(IsPrimaryKey = false, Storage = "_wb_wasta")]
        public int wb_wasta
        {
            get
            {
                return _wb_wasta;
            }

            set
            {
                _wb_wasta = value;
            }
        } //The real question is: Is this all the columns we need?

        private DateTime _wb_date;
        [Column(IsPrimaryKey = false, Storage = "_wb_date")] //Unsure about column name here too, mcmain.wachtboek uses english, and mcmain.acaf uses dutch. Or the other way around, I forgot :/
        public DateTime wb_date
        {
            get
            {
                return _wb_date;
            }

            set
            {
                _wb_date = value;
            }
        } //Answer to previous question: NO, we needed this one too :/ (Duh)
    }
}
