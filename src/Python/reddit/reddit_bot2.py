import praw
import time
import redit_bot_config



def bot_login():
	print "Loggin in..."
	r = praw.Reddit(
		username = redit_bot_config.username,
		password = redit_bot_config.password,
		client_id = redit_bot_config.key,
		client_secret = redit_bot_config.secret,
		user_agent = "")
	print("Logged in.")

	return r

def run_bot(r):
	print("Obtaining 25 comments...")
	for comment in r.subreddit('see_sharp_dotnet_dev').comments(limit=25):
		if "poop" in comment.body:
			print("String with \"poop\" found in comment " + comment.id)
			comment.reply("lol poop")
			print("Replied to comment " + comment.id)

	time.sleep(10)

r = bot_login()

while True:
	run_bot(r)