using System;
using Godot;

namespace RootGame;

public partial class Level : Node2D
{
	[Signal]
	public delegate void RestartGameEventHandler();

	private const int MaxSaturation = 90;
	
	private int _score;
	private int _saturation = MaxSaturation;
	private bool _isGameOver;
	
	public int Score
	{
		get => _score;
		private set
		{
			_score = value;
			_scoreLabel.Text = FormatScore(value);
		}
	}
	
	public int Saturation
	{
		get => _saturation;
		private set
		{
			_saturation = value > MaxSaturation ? MaxSaturation : value;
			_progressBar.Value = _saturation;
			
			if (value <= 0)
			{
				DoGameOver();
			}
		}
	}
	private TextureProgressBar _progressBar;
	private Label _scoreLabel;
	private Timer _resourceTimer;
	private Timer _monsterTimer;
	private Timer _scoreTimer;
	private GodotObject[] _monsters =  new GodotObject[6];

	public override void _Ready()
	{
		_progressBar = GetNode<TextureProgressBar>("TextureProgressBar") ?? throw new NullReferenceException("No TextureProgressBar for level");
		_progressBar.MaxValue = MaxSaturation;
		_scoreLabel = GetNode<Label>("ScoreLabel") ?? throw new NullReferenceException("No ScoreLabel for level");
		_scoreLabel.Text = FormatScore(Score);
		_resourceTimer = GetNode<Timer>("ResourceTimer") ?? throw new NullReferenceException("No ResourceTimer for level");
		_monsterTimer = GetNode<Timer>("MonsterTimer") ?? throw new NullReferenceException("No MonsterTimer for level");
		_scoreTimer = GetNode<Timer>("ScoreTimer") ?? throw new NullReferenceException("No ScoreTimer for level");
		for (var i = 0; i < 6; i++)
		{
			_monsters[i] = GetNode<GodotObject>("Monster" + i);
		}
		
		
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (!_isGameOver)
		{
			return;
		}

		if (@event.IsActionPressed("restart"))
		{
			EmitSignalRestartGame();
		}
	}

	public void DoGameOver()
	{
		_isGameOver = true;
		
		_resourceTimer.Stop();
		_monsterTimer.Stop();
		_scoreTimer.Stop();
		
		GetNode<AudioStreamPlayer>("MusicSound").Stop();
		GetNode<AudioStreamPlayer>("GameOverSound").Play();

		
		var gameOverCard = GetNode<Node2D>("GameOverCard");
		gameOverCard.GetNode<Label>("ScoreLabel").Text = FormatScore(Score);
		gameOverCard.Show();
	}

	private void OnResourceTimerTimeout()
	{
		var resourceRandom = GD.Randi() % 5;
		var resource = resourceRandom switch
		{
			< 1 => CreateSporeScene(),
			< 3 => CreateEnergyScene(),
			_ => CreateWaterScene()
		};
		
		var interval = 1 + GD.Randf() % 7 ;
		_resourceTimer.Start(interval);
		
		AddChild(resource);
	}

	private void OnMonsterTimerTimeout()
	{
		var monsterRandom = GD.Randi() % 5;
		_monsters[monsterRandom].Call("activate");
		
		var interval = 5 + GD.Randi() % 15 ;
		_monsterTimer.Start(interval);
	}

	private void OnScoreTimerTimeout()
	{
		Score++;
		Saturation--;
	}

	private string FormatScore(int score)
	{
		return $"{score:D4}";
	}

	private Node CreateSporeScene()
	{
		var spore = CreateResourceScene(Resource.SporeScene);
		spore.Connect("spore_planted", Callable.From<Vector2>(position =>
		{
			var mushroom = GD.Load<PackedScene>("res://src/mushroom.tscn").Instantiate<Node2D>();
			mushroom.Set("position", position);
			AddChild(mushroom);
			mushroom.Connect("monster_fed", Callable.From(() =>
			{
				Saturation += 10;
				_progressBar.Value = Saturation;
			}));
		}));
		
		return spore;
	}

	private Node CreateEnergyScene()
	{
		return CreateResourceScene(Resource.EnergyScene);
	}

	private Node CreateWaterScene()
	{
		return CreateResourceScene(Resource.WaterScene);
	}

	private Node CreateResourceScene(PackedScene resourceScene)
	{
		var scene = resourceScene.Instantiate<Node>();
		var spawnLocation = GetNode<PathFollow2D>("ResourceSpawner/Path");
		spawnLocation.ProgressRatio = GD.Randf();
		scene.Set("position", spawnLocation.Position);
		return scene;
	}
}