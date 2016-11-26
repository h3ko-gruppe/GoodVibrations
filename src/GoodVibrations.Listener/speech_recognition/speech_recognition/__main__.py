import speech_recognition as sr
import httplib, urllib

# TODOs
# Create function to record reference sound file

def sendTagToWebService():
    params = urllib.urlencode({"eventId" : "sound1" })
    print params
    headers = {"Content-Type": "application/json"}
    conn = httplib.HTTPSConnection("goodvibrations-app.azurewebsites.net")
    conn.request("POST", "/api/notify", params, headers)
    response = conn.getresponse()
    print response.status, response.reason
    data = response.read()
    conn.close()

def recordReferenceSound():
    recordedSound = False

    while recordedSound != True:
        print("Listening for reference sound...")
        with m as source: audio = r.listen(source)
        print("Got it! Now to recognize it...")

        try:
            #target = open('recorded_data.pcm','w')

            sendTagToWebService()

            #target.close()

            recordedSound = True
        except sr.UnknownValueError:
            print("Oops! Didn't catch that")
        except sr.RequestError as e:
            print("Uh oh! Couldn't request results from Google Speech Recognition service; {0}".format(e))

r = sr.Recognizer()
m = sr.Microphone()

try:
    print("A moment of silence, please...")
    with m as source: r.adjust_for_ambient_noise(source)
    print("Set minimum energy threshold to {}".format(r.energy_threshold))

    recordReferenceSound()

    # Good vibrations
    # - App für Hörgeschädigte
    # Helfen, Geräte im Haushalt wahrzunehmen, die sie akustisch bemerkar machen
    # vibrierendes Armband
    # kostengünstig
    # kein Umbau#

    # Wir sind das Team vom Projekt Good vibr.
    # Mit Hilfe unserer App können Hörgeschädigte über so ein vibrierendes Armband
    # überall in ihrer Wohnung darüber informiert werden wenn es z.B. an der Tür geklingelt hat
    # und sie verpassen dadurch nie wieder Besuch oder den Paketboten.


    #while True:
    #    print("Say something to me")
    #    with m as source: audio = r.listen(source)
    #    print("Got it! Now to recognize it...")
    #    try:
    #        print("bla")
            #target.write('test')

            #target.write(audio.get_raw_data())

            #print(elem.encode("hex") for elem in audio.get_raw_data())

            # recognize speech using Google Speech Recognition
            # value = r.recognize_google(audio)

            #target.close()

            # we need some special handling here to correctly print unicode characters to standard output
            #if str is bytes:  # this version of Python uses bytes for strings (Python 2)
            #     print(u"You said {}".format(value).encode("utf-8"))
            #else:  # this version of Python uses unicode for strings (Python 3+)
            #    print("You said {}".format(value))
    #    except sr.UnknownValueError:
    #        print("Oops! Didn't catch that")
    #    except sr.RequestError as e:
    #        print("Uh oh! Couldn't request results from Google Speech Recognition service; {0}".format(e))
except KeyboardInterrupt:
    pass
