﻿using Blamite.Blam;
using Blamite.Blam.Scripting;
using Blamite.Blam.Util;
using Blamite.Serialization.Settings;

namespace Blamite.Serialization
{
	/// <summary>
	///     Describes information about a supported engine.
	/// </summary>
	public class EngineDescription
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="EngineDescription" /> class.
		/// </summary>
		/// <param name="name">The engine's name.</param>
		/// <param name="version">The engine's version.</param>
		/// <param name="settings">The engine's settings.</param>
		public EngineDescription(string name, string version, SettingsGroup settings)
		{
			Name = name;
			Version = version;
			Settings = settings;

			LoadSettings();
		}

		/// <summary>
		///     Gets the name of the engine.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		///     Gets the engine's version string.
		/// </summary>
		public string Version { get; private set; }

		/// <summary>
		///     Gets the settings for the engine.
		/// </summary>
		public SettingsGroup Settings { get; private set; }

		/// <summary>
		///     Gets the size of a map header.
		/// </summary>
		public int HeaderSize { get; private set; }

		/// <summary>
		///     Gets the value to align segment boundaries on.
		/// </summary>
		public int SegmentAlignment { get; private set; }

		/// <summary>
		///     Gets the key to decrypt tagnames with.
		///     Can be <c>null</c> if decryption is not required.
		/// </summary>
		public AESKey TagNameKey { get; private set; }

		/// <summary>
		///     Gets the key to decrypt stringIDs with.
		///     Can be <c>null</c> if decryption is not required.
		/// </summary>
		public AESKey StringIDKey { get; private set; }

		/// <summary>
		///     Gets the key to decrypt localized strings with.
		///     Can be <c>null</c> if decryption is not required.
		/// </summary>
		public AESKey LocaleKey { get; private set; }

		/// <summary>
		///     Gets the layouts for the engine.
		///     Can be <c>null</c> if not present.
		/// </summary>
		public StructureLayoutCollection Layouts { get; private set; }

		/// <summary>
		///     Gets the stringID set resolver for the engine.
		///     Can be <c>null</c> if not present.
		/// </summary>
		public StringIDNamespaceResolver StringIDs { get; private set; }

		/// <summary>
		///     Gets scripting info for the engine.
		///     Can be <c>null</c> if not present.
		/// </summary>
		public OpcodeLookup ScriptInfo { get; private set; }

		/// <summary>
		///     Gets locale symbols for the engine.
		///     Can be <c>null</c> if not present.
		/// </summary>
		public LocaleSymbolCollection LocaleSymbols { get; private set; }

		/// <summary>
		///     Gets vertex layouts for the engine.
		///     Can be <c>null</c> if not present.
		/// </summary>
		public VertexLayoutCollection VertexLayouts { get; private set; }

		/// <summary>
		///     Gets group names for the engine.
		///     Can be <c>null</c> if not present.
		/// </summary>
		public GroupNameCollection GroupNames { get; private set; }

		/// <summary>
		///		Gets the magic number used to expand tag addresses for the engine
		/// </summary>
		public int ExpandMagic { get; private set; }

		/// <summary>
		///     Gets the name of the game executable for poking purposes.
		///     Can be <c>null</c> if not present.
		/// </summary>
		public string GameExecutable { get; private set; }

		/// <summary>
		///     Gets the name of the alternate game executable for poking purposes. (win store)
		///     Can be <c>null</c> if not present.
		/// </summary>
		public string GameExecutableAlt { get; private set; }

		/// <summary>
		///     Gets the name of the engine's module for poking purposes.
		///     Can be <c>null</c> if not present.
		/// </summary>
		public string GameModule { get; private set; }

		/// <summary>
		///     Gets the collection of pointers to allow for poking on PC.
		///     Can be <c>null</c> if not present.
		/// </summary>
		public PokingCollection Poking { get; private set; }

		/// <summary>
		///		The offset past the header from a poking xml pointer to the value to add for address conversion.
		/// </summary>
		public int PokingOffset { get; private set; }

		/// <summary>
		///		Whether the cache file itself uses compression.
		/// </summary>
		public bool UsesCompression { get; private set; }

		/// <summary>
		///		MCC sometimes ships maps with string hashes 0'd out, in some cases adding a hash can cause issues.
		/// </summary>
		public bool UsesStringHashes { get; private set; }

		/// <summary>
		///		MCC sometimes ships maps with resource hashes and checksums 0'd out, this makes some injection optimizations impossible.
		/// </summary>
		public bool UsesRawHashes { get; private set; }

		/// <summary>
		///		Some/later builds have optimization which removes unused shader code from maps to save space. Injection can mean these removed shaders can be referenced and cause issues.
		/// </summary>
		public bool OptimizedShaders { get; private set; }

		private void LoadSettings()
		{
			LoadEngineSettings();
			LoadDatabases();
		}

		private void LoadEngineSettings()
		{
			HeaderSize = Settings.GetSetting<int>("engineInfo/headerSize");
			SegmentAlignment = Settings.GetSettingOrDefault("engineInfo/segmentAlignment", 0x1000);
			ExpandMagic = Settings.GetSettingOrDefault("engineInfo/expandMagic", -1);

			UsesCompression = Settings.GetSettingOrDefault("engineInfo/usesCompression", false);
			UsesStringHashes = Settings.GetSettingOrDefault("engineInfo/usesStringHashes", true);
			UsesRawHashes = Settings.GetSettingOrDefault("engineInfo/usesRawHashes", true);
			OptimizedShaders = Settings.GetSettingOrDefault("engineInfo/optimizedShaders", false);

			if (Settings.PathExists("engineInfo/pokingOffset"))
				PokingOffset = Settings.GetSettingOrDefault("engineInfo/pokingOffset", 0);

			if (Settings.PathExists("engineInfo/gameExecutable"))
				GameExecutable = Settings.GetSetting<string>("engineInfo/gameExecutable");
			if (Settings.PathExists("engineInfo/gameExecutableAlt"))
				GameExecutableAlt = Settings.GetSetting<string>("engineInfo/gameExecutableAlt");
			if (Settings.PathExists("engineInfo/gameModule"))
				GameModule = Settings.GetSetting<string>("engineInfo/gameModule");
			if (Settings.PathExists("engineInfo/encryption/tagNameKey"))
				TagNameKey = new AESKey(Settings.GetSettingOrDefault<string>("engineInfo/encryption/tagNameKey", null));
			if (Settings.PathExists("engineInfo/encryption/stringIdKey"))
				StringIDKey = new AESKey(Settings.GetSettingOrDefault<string>("engineInfo/encryption/stringIdKey", null));
			if (Settings.PathExists("engineInfo/encryption/localeKey"))
				LocaleKey = new AESKey(Settings.GetSettingOrDefault<string>("engineInfo/encryption/localeKey", null));
		}

		private void LoadDatabases()
		{
			Layouts = Settings.GetSettingOrDefault<StructureLayoutCollection>("databases/layouts", null);
			StringIDs = Settings.GetSettingOrDefault<StringIDNamespaceResolver>("databases/stringIds", null);
			ScriptInfo = Settings.GetSettingOrDefault<OpcodeLookup>("databases/scripting", null);
			LocaleSymbols = Settings.GetSettingOrDefault<LocaleSymbolCollection>("databases/localeSymbols", null);
			VertexLayouts = Settings.GetSettingOrDefault<VertexLayoutCollection>("databases/vertexLayouts", null);
			GroupNames = Settings.GetSettingOrDefault<GroupNameCollection>("databases/groupNames", null);
			Poking = Settings.GetSettingOrDefault<PokingCollection>("databases/poking", null);
		}
	}
}