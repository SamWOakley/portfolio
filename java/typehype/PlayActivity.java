package kings.enterprise.typehype;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.LineNumberReader;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;
import java.util.List;
import java.util.Random;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.SharedPreferences.Editor;
import android.content.res.Resources;
import android.graphics.Color;
import android.os.Bundle;
import android.os.Handler;
import android.os.SystemClock;
import android.view.KeyEvent;
import android.view.Window;
import android.view.WindowManager;
import android.view.animation.Animation;
import android.view.animation.AnimationUtils;
import android.view.inputmethod.EditorInfo;
import android.widget.ArrayAdapter;
import android.widget.BaseAdapter;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.TextView.OnEditorActionListener;

public class PlayActivity extends Activity {

	private EditText answerTextField;
	private Handler handler = new Handler();
	private Animation listRemoveAnimation, listAdditionAnimation;
	private long previousTime;
	private ProgressBar progressBar;
	private Random random;
	private int score = 0;
	private TextView scoreTextView;
	private long timeRemaining;
	private Runnable updateTimeTask;
	private ArrayList<String> wordsBuffer;
	private List<String> wordsList;
	private ListView wordsListView;

	protected boolean checkAnswer() {

		boolean correct = false;

		String answer = answerTextField.getText().toString();
		answerTextField.setText("");
		for (int i = 0; i < wordsList.size(); i++) {
			String toCheck = wordsList.get(i);
			if (toCheck.equalsIgnoreCase(answer)) {
				score += toCheck.length() * Constants.SCORE_PER_LETTER;
				scoreTextView.setText(String.valueOf(score));

				// start animation and handler for removal

				final int index = i;

				wordsListView.getChildAt(index).startAnimation(
						listRemoveAnimation);
				handler.postDelayed(new Runnable() {

					@Override
					public void run() {
						replaceWordAt(index);
					}
				}, listRemoveAnimation.getDuration());

				correct = true;

			}
		}

		if (wordsBuffer.size() <= 0) {
			fillBuffer();
		}

		return correct;
	}

	protected void endGame() {
		
		handler.removeCallbacksAndMessages(null);

		//set up dialog for name entry and choices
		
		AlertDialog.Builder builder = new AlertDialog.Builder(this);
		builder.setMessage("Your score was: " + score);

		final EditText nameInput = new EditText(this);
		builder.setView(nameInput);

		builder.setPositiveButton("Play again",
				new DialogInterface.OnClickListener() {

					@Override
					public void onClick(DialogInterface dialog, int which) {
						if (nameInput.getText().toString() == "")
							return;
						startActivity(new Intent(PlayActivity.this,
								PlayActivity.class));
						saveHighScore(nameInput.getText().toString());
						finish();
					}
				});
		builder.setNegativeButton("Main menu",
				new DialogInterface.OnClickListener() {

					@Override
					public void onClick(DialogInterface dialog, int which) {
						if (nameInput.getText().toString() == "")
							return;
						saveHighScore(nameInput.getText().toString());
						finish();
					}
				});

		AlertDialog dialog = builder.create();
		dialog.setCanceledOnTouchOutside(false);
		dialog.show();
	}

	private void fillBuffer() {
		wordsBuffer.clear();
		List<Integer> lineNumbers = new ArrayList<Integer>(
				Constants.WORDS_TO_SHOW);

		BufferedReader bufferedReader = null;
		Resources res = getResources();

		try {
			bufferedReader = new BufferedReader(new InputStreamReader(res
					.getAssets().open(Constants.WORDS_FILE_NAME)));

			LineNumberReader lnr = new LineNumberReader(bufferedReader);
			lnr.skip(Long.MAX_VALUE);

			int fileLength = lnr.getLineNumber();

			for (int i = wordsList.size(); i < Constants.WORD_BUFFER_SIZE; i++) {
				lineNumbers.add(random.nextInt(fileLength) + 1);
			}

			bufferedReader = new BufferedReader(new InputStreamReader(res
					.getAssets().open(Constants.WORDS_FILE_NAME)));

			String currentLine;
			int lineCount = 1;
			while ((currentLine = bufferedReader.readLine()) != null) {
				++lineCount;

				for (int i = 0; i < lineNumbers.size(); i++) {
					int wantedLine = lineNumbers.get(i);
					if (wantedLine == lineCount - 1) {
						wordsBuffer.add(currentLine);
					}
				}

			}

		} catch (IOException e) {
			e.printStackTrace();
		} finally {
			if (bufferedReader != null)
				try {
					bufferedReader.close();
				} catch (IOException e) {
					e.printStackTrace();
				}
		}
	}

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);

		requestWindowFeature(Window.FEATURE_NO_TITLE);
		getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN,
				WindowManager.LayoutParams.FLAG_FULLSCREEN);

		setContentView(R.layout.activity_play);

		random = new Random();

		updateTimeTask = new Runnable() {

			@Override
			public void run() {
				long currentTime = System.currentTimeMillis();
				timeRemaining -= (currentTime - previousTime);
				previousTime = currentTime;

				progressBar.incrementProgressBy(1);
				progressBar
						.setProgress((int) (Constants.TIME_LIMIT - timeRemaining));

				if (timeRemaining < 0) {
					endGame();
					return;
				}

				handler.postAtTime(updateTimeTask, SystemClock.uptimeMillis()
						+ Constants.TIMER_UPDATE_INTERVAL);
			}
		};

		listRemoveAnimation = AnimationUtils.loadAnimation(this,
				android.R.anim.slide_out_right);
		listRemoveAnimation.setDuration(Constants.ANIMATION_TIME);
		listAdditionAnimation = AnimationUtils.loadAnimation(this,
				android.R.anim.slide_in_left);
		listAdditionAnimation.setDuration(Constants.ANIMATION_TIME);

		wordsList = new ArrayList<String>(4);
		wordsBuffer = new ArrayList<String>();
		wordsListView = (ListView) findViewById(R.id.words_listview);
		ArrayAdapter<String> adapter = new ArrayAdapter<String>(this,
				R.layout.play_list_item, wordsList);
		wordsListView.setAdapter(adapter);

		scoreTextView = (TextView) findViewById(R.id.score_text);

		answerTextField = (EditText) findViewById(R.id.answer_textfield);
		answerTextField.setOnEditorActionListener(new OnEditorActionListener() {

			@Override
			public boolean onEditorAction(TextView v, int actionId,
					KeyEvent event) {
				if (actionId == EditorInfo.IME_ACTION_DONE) {
					if (answerTextField.getText().toString().trim().length() > 0) {
						boolean correctAnswer = checkAnswer();
						answerTextField
								.setBackgroundColor((correctAnswer) ? Constants.CORRECT_COLOR
										: Constants.INCORRECT_COLOR);
						handler.postDelayed(new Runnable() {

							@Override
							public void run() {
								answerTextField.setBackgroundColor(Color.TRANSPARENT);
							}
						}, Constants.ANIMATION_TIME);
						timeRemaining += (correctAnswer) ? Constants.ANSWER_BONUS_TIME
								: -Constants.ANSWER_BONUS_TIME;
					}
					return true;
				} else {
					return false;
				}
			}
		});
		answerTextField.requestFocus();

		progressBar = (ProgressBar) findViewById(R.id.time_remaining_bar);
		progressBar.setMax(Constants.TIME_LIMIT);

		previousTime = System.currentTimeMillis();
		timeRemaining = Constants.TIME_LIMIT;

		handler.post(updateTimeTask);

		fillBuffer();

		for (int i = 0; i < Constants.WORDS_TO_SHOW; i++) {
			wordsList.add(wordsBuffer.get(0));
			wordsBuffer.remove(0);
		}

		((BaseAdapter) wordsListView.getAdapter()).notifyDataSetChanged();

		scoreTextView.setText(String.valueOf(score));
	}

	//TODO get stopping a resuming working properly
	@Override
	protected void onStop() {

		//handler.removeCallbacks(updateTimeTask);
		handler.removeCallbacksAndMessages(null);

		super.onStop();
	}

	private void replaceWordAt(int index) {
		wordsList.set(index, wordsBuffer.get(0));
		wordsBuffer.remove(0);
		wordsListView.getChildAt(index).startAnimation(listAdditionAnimation);

		((BaseAdapter) wordsListView.getAdapter()).notifyDataSetChanged();
	}

	private void saveHighScore(String nameInput) {

		ArrayList<HighScore> highScores = new ArrayList<HighScore>();

		SharedPreferences scorePrefs = this.getSharedPreferences("highscores",
				Context.MODE_PRIVATE);
		SharedPreferences namePrefs = this.getSharedPreferences(
				"highscoresnames", Context.MODE_PRIVATE);
		for (int i = 0; i < Constants.HIGH_SCORES_TO_SAVE; i++) {
			HighScore h = new HighScore(scorePrefs.getInt(String.valueOf(i), 0),
					namePrefs.getString(String.valueOf(i), ""));

			if (h.playerName != "" && h.score > 0)
				highScores.add(h);
		}

		highScores.add(new HighScore(score, nameInput));

		if (highScores.size() > 1) {
			if (highScores.size() > Constants.HIGH_SCORES_TO_SAVE) {
				int minScoreIndex = -1;
				int minScore = -1;
				for (HighScore highScore : highScores) {
					if (highScore.score < minScore || minScore < 0) {
						minScore = highScore.score;
						minScoreIndex = highScores.indexOf(highScore);
					}
				}
				highScores.remove(minScoreIndex);
			}

			Collections.sort(highScores, new Comparator<HighScore>() {

				@Override
				public int compare(HighScore arg0, HighScore arg1) {

					return (arg1.score - arg0.score);
				}

			});
		}

		Editor scoreEditor = scorePrefs.edit();
		Editor nameEditor = namePrefs.edit();
		scoreEditor.clear();
		nameEditor.clear();

		for (int i = 0; i < highScores.size(); i++) {
			scoreEditor.putInt(String.valueOf(i), highScores.get(i).score);
			nameEditor.putString(String.valueOf(i), highScores.get(i).playerName);
		}
		scoreEditor.commit();
		nameEditor.commit();

	}

}
