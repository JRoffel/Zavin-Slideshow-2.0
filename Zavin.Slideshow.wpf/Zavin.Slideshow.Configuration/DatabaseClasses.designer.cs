﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Zavin.Slideshow.Configuration
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="mczavidord")]
	public partial class DatabaseClassesDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void Insertconfig(config instance);
    partial void Updateconfig(config instance);
    partial void Deleteconfig(config instance);
    #endregion
		
		public DatabaseClassesDataContext() : 
				base(global::Zavin.Slideshow.Configuration.Properties.Settings.Default.mczavidordConnectionString1, mappingSource)
		{
			OnCreated();
		}
		
		public DatabaseClassesDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DatabaseClassesDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DatabaseClassesDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DatabaseClassesDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<config> configs
		{
			get
			{
				return this.GetTable<config>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="mcmain.config")]
	public partial class config : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private System.Nullable<int> _YearTargetTon;
		
		private System.Nullable<int> _SlideTimerSeconds;
		
		private System.Nullable<int> _MemoRunCounter;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnYearTargetTonChanging(System.Nullable<int> value);
    partial void OnYearTargetTonChanged();
    partial void OnSlideTimerSecondsChanging(System.Nullable<int> value);
    partial void OnSlideTimerSecondsChanged();
    partial void OnMemoRunCounterChanging(System.Nullable<int> value);
    partial void OnMemoRunCounterChanged();
    #endregion
		
		public config()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_YearTargetTon", DbType="Int")]
		public System.Nullable<int> YearTargetTon
		{
			get
			{
				return this._YearTargetTon;
			}
			set
			{
				if ((this._YearTargetTon != value))
				{
					this.OnYearTargetTonChanging(value);
					this.SendPropertyChanging();
					this._YearTargetTon = value;
					this.SendPropertyChanged("YearTargetTon");
					this.OnYearTargetTonChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SlideTimerSeconds", DbType="Int")]
		public System.Nullable<int> SlideTimerSeconds
		{
			get
			{
				return this._SlideTimerSeconds;
			}
			set
			{
				if ((this._SlideTimerSeconds != value))
				{
					this.OnSlideTimerSecondsChanging(value);
					this.SendPropertyChanging();
					this._SlideTimerSeconds = value;
					this.SendPropertyChanged("SlideTimerSeconds");
					this.OnSlideTimerSecondsChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MemoRunCounter", DbType="Int")]
		public System.Nullable<int> MemoRunCounter
		{
			get
			{
				return this._MemoRunCounter;
			}
			set
			{
				if ((this._MemoRunCounter != value))
				{
					this.OnMemoRunCounterChanging(value);
					this.SendPropertyChanging();
					this._MemoRunCounter = value;
					this.SendPropertyChanged("MemoRunCounter");
					this.OnMemoRunCounterChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
