using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using alpoLib.Res;
using alpoLib.Util;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MergeBoard.Sound
{
	public enum BGMKey
	{
		None = 0,
		
		bgm_main,
		bgm_alphabet_board,
		bgm_number_board,
	}

	public enum SFXKey
	{
		None = 0,
		
		sfx_splash,
		sfx_board_item_pop,
		sfx_board_item_merge,
		sfx_success,
		sfx_click,
		sfx_cancel,
		sfx_sell,
	}

	internal enum BGMChannel
	{
		None = 0,
		
		BGM_1,
		BGM_2,

		MAX,
	}

	internal enum SFXChannel
	{
		None = 0,

		SFX_1,
		SFX_2,
		SFX_3,
		SFX_4,
		SFX_5,
		SFX_6,
		SFX_7,
		SFX_8,
		SFX_9,
		SFX_10,

		MAX,
	}

	internal class SoundManager : SingletonMonoBehaviour<SoundManager>
	{
		private readonly Dictionary<BGMChannel, AudioSource> _bgmChannel = new();
		private readonly Dictionary<SFXChannel, AudioSource> _sfxChannel = new();
		
		private readonly Dictionary<string, AudioClip> _bgmCache = new();
		private readonly Dictionary<string, AudioClip> _sfxCache = new();

		private BGMChannel _currentBGMChannel = BGMChannel.None;
		private SFXChannel _currentSFXChannel = SFXChannel.None;

		private BGMChannel _nextBGMChannel = BGMChannel.None;
		private SFXChannel _nextSFXChannel = SFXChannel.None;

		private void CreateChannels()
		{
			_bgmChannel.Clear();
			for (int i = (int)BGMChannel.None + 1; i < (int)BGMChannel.MAX; i++)
			{
				var go = new GameObject($"BGM_{(BGMChannel)i}");
				go.transform.SetParentEx(transform);
				var source = go.AddComponent<AudioSource>();
				source.playOnAwake = false;
				source.spatialBlend = 0f;
				source.spread = 180f;
				source.rolloffMode = AudioRolloffMode.Linear;
				_bgmChannel.Add((BGMChannel)i, source);
			}

			_sfxChannel.Clear();
			for (int i = (int)SFXChannel.None + 1; i < (int)SFXChannel.MAX; i++)
			{
				var go = new GameObject($"SFX_{(SFXChannel)i}");
				go.transform.SetParentEx(transform);
				var source = go.AddComponent<AudioSource>();
				source.playOnAwake = false;
				source.spatialBlend = 0f;
				source.spread = 180f;
				source.rolloffMode = AudioRolloffMode.Linear;
				_sfxChannel.Add((SFXChannel)i, source);
			}
		}

		public async Awaitable PreloadAsync()
		{
			CreateChannels();
			await PreloadBGMAsync();
			await PreloadSFXAsync();
		}
		
		private async Awaitable PreloadBGMAsync()
		{
			var bgms = Enum.GetValues(typeof(BGMKey));
			var handleList = new List<AsyncOperationHandle<AudioClip>>();
			foreach (BGMKey key in bgms)
			{
				if (key == BGMKey.None)
					continue;
				var addrKey = $"Addr/Sound/BGM/{key}";
				var h = Addressables.LoadAssetAsync<AudioClip>(addrKey);
				h.Completed += handle =>
				{
					_bgmCache.Add(addrKey, handle.Result);
				};
				handleList.Add(h);
			}

			await AddressableLoaderHelper.WhenAll(handleList.ToArray());
		}
		
		private async Awaitable PreloadSFXAsync()
		{
			var sfxs = Enum.GetValues(typeof(SFXKey));
			var handleList = new List<AsyncOperationHandle<AudioClip>>();
			foreach (SFXKey key in sfxs)
			{
				if (key == SFXKey.None)
					continue;
				var addrKey = $"Addr/Sound/SFX/{key}";
				var h = Addressables.LoadAssetAsync<AudioClip>(addrKey);
				h.Completed += handle =>
				{
					_sfxCache.Add(addrKey, handle.Result);
				};
				handleList.Add(h);
			}

			await AddressableLoaderHelper.WhenAll(handleList.ToArray());
		}

		public void PlayBGM(BGMKey key, bool loop)
		{
			if (key == BGMKey.None)
				return;

			var addrKey = $"Addr/Sound/BGM/{key}";
			if (!_bgmCache.TryGetValue(addrKey, out var clip))
			{
				Debug.LogWarning($"BGM not found: {addrKey}");
				return;
			}

			if (_nextBGMChannel == BGMChannel.None)
				_nextBGMChannel = BGMChannel.BGM_1;

			StopBGM(_currentBGMChannel);

			if (!_bgmChannel.TryGetValue(_nextBGMChannel, out var source))
				return;

			source.rolloffMode = AudioRolloffMode.Linear;
			source.loop = loop;
			source.clip = clip;
			source.Play();

			_currentBGMChannel = _nextBGMChannel;
			_nextBGMChannel++;
			if (_nextBGMChannel >= BGMChannel.MAX)
				_nextBGMChannel = BGMChannel.BGM_1;
		}

		public void StopBGM(BGMChannel channel = BGMChannel.MAX)
		{
			if (channel == BGMChannel.MAX)
			{
				foreach (var source in _bgmChannel)
				{
					source.Value.Stop();
				}
			}
			else if (_bgmChannel.TryGetValue(channel, out var source))
				source.Stop();
		}

		public bool IsPlayingBGM()
		{
			if (_bgmChannel.TryGetValue(_nextBGMChannel, out var source))
				return source.isPlaying;
			return false;
		}

		public void PlaySFX(SFXKey key, bool loop = false)
		{
			if (key == SFXKey.None)
				return;

			var addrKey = $"Addr/Sound/SFX/{key}";
			if (!_sfxCache.TryGetValue(addrKey, out var clip))
			{
				Debug.LogWarning($"SFX not found: {addrKey}");
				return;
			}

			if (_nextSFXChannel == SFXChannel.None)
				_nextSFXChannel = SFXChannel.SFX_1;

			if (!_sfxChannel.TryGetValue(_nextSFXChannel, out var source))
				return;

			source.rolloffMode = AudioRolloffMode.Linear;
			source.loop = loop;
			source.clip = clip;
			source.Play();

			_currentSFXChannel = _nextSFXChannel;
			_nextSFXChannel++;
			if (_nextSFXChannel >= SFXChannel.MAX)
				_nextSFXChannel = SFXChannel.SFX_1;
		}

		public bool IsPlayingSFX()
		{
			if (_sfxChannel.TryGetValue(_nextSFXChannel, out var source))
				return source.isPlaying;
			return false;
		}
	}
}