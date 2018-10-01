package kings.enterprise.typehype;

import java.util.ArrayList;

import android.app.Activity;
import android.content.Context;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.Menu;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.view.View.OnClickListener;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ListView;

public class HighScoresActivity extends Activity {

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		
		requestWindowFeature(Window.FEATURE_NO_TITLE);
		getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN,
				WindowManager.LayoutParams.FLAG_FULLSCREEN);
		
		setContentView(R.layout.activity_high_scores);

		ListView listView = (ListView) findViewById(R.id.highscores_list);

		ArrayAdapter<String> adapter = new ArrayAdapter<String>(this,
				R.layout.highscores_list_item, android.R.id.text1,
				loadScores());
		listView.setAdapter(adapter);

		Button menuButton = (Button) findViewById(R.id.menu_button);
		menuButton.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				HighScoresActivity.this.finish();
			}
		});

	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.activity_high_scores, menu);
		return true;
	}

	private String[] loadScores() {
		ArrayList<HighScore> highScores = new ArrayList<HighScore>();

		SharedPreferences scorePrefs = this.getSharedPreferences("highscores",
				Context.MODE_PRIVATE);
		SharedPreferences namePrefs = this.getSharedPreferences(
				"highscoresnames", Context.MODE_PRIVATE);
		for (int i = 0; i < Constants.HIGH_SCORES_TO_SAVE; i++) {
			HighScore h = new HighScore(scorePrefs.getInt("" + i, 0),
					namePrefs.getString("" + i, ""));

			if (h.playerName != "")
				highScores.add(h);
		}

		String highScoresString[] = new String[highScores.size()];
		for (int i = 0; i < highScores.size(); i++) {
			highScoresString[i] = i + ": " + highScores.get(i).playerName + " "
					+ highScores.get(i).score;
		}

		return highScoresString;
	}

}
