package kings.enterprise.typehype;

import android.os.Bundle;
import android.app.Activity;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.view.View.OnClickListener;
import android.widget.Button;

public class HelpActivity extends Activity {

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		
		requestWindowFeature(Window.FEATURE_NO_TITLE);
		getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN,
				WindowManager.LayoutParams.FLAG_FULLSCREEN);
		
		setContentView(R.layout.activity_help);

		
		Button menuButton = (Button) findViewById(R.id.menu_button);
		menuButton.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				HelpActivity.this.finish();
			}
		});
	}

}
