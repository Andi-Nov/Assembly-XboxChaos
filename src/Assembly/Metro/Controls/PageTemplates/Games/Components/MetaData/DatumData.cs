﻿namespace Assembly.Metro.Controls.PageTemplates.Games.Components.MetaData
{
	/// <summary>
	///     32-bit datum, split into an index and salt
	/// </summary>
	public class DatumData : ValueField
	{
		private ushort _salt;
		private ushort _index;

		public DatumData(string name, uint offset, long address, ushort salt, ushort index, uint pluginLine, string tooltip)
			: base(name, offset, address, pluginLine, tooltip)
		{
			_salt = salt;
			_index = index;
		}

		public ushort Salt
		{
			get { return _salt; }
			set
			{
				_salt = value;
				NotifyPropertyChanged("Salt");
			}
		}

		public ushort Index
		{
			get { return _index; }
			set
			{
				_index = value;
				NotifyPropertyChanged("Index");
			}
		}

		public override void Accept(IMetaFieldVisitor visitor)
		{
			visitor.VisitDatum(this);
		}

		public override MetaField CloneValue()
		{
			return new DatumData(Name, Offset, FieldAddress, Salt, Index, PluginLine, ToolTip);
		}
	}
}
