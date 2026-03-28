import requests

BASE_URL = "http://localhost:5000/api/messages"  # change host/port if needed
USERNAME = "admin"
PASSWORD = "password123"
TEST_PGN = "01F112"
TEST_TIME = 60  # seconds

def test_endpoint(description, url):
    print(f"\n=== {description} ===")
    try:
        resp = requests.get(url)
        print(f"URL: {url}")
        print(f"Status Code: {resp.status_code}")
        try:
            print(f"Response JSON: {resp.json()}")
        except:
            print(f"Response Text: {resp.text}")
    except Exception as e:
        print(f"Error: {e}")

if __name__ == "__main__":
    test_endpoint("1. Login", f"{BASE_URL}/login/{USERNAME}/{PASSWORD}")
    test_endpoint("2. Status Check", f"{BASE_URL}/status")
    test_endpoint("3. Get All Messages", f"{BASE_URL}/all")
    test_endpoint("4. Get Total Message Count", f"{BASE_URL}/total")
    test_endpoint("5. Get New Messages Since Time", f"{BASE_URL}/new/{TEST_TIME}")
    test_endpoint("6. Get Messages by PGN", f"{BASE_URL}/by-pgn/{TEST_PGN}")
    test_endpoint("7. Get Messages by PGN Since Time", f"{BASE_URL}/by-pgn/{TEST_PGN}/since/{TEST_TIME}")
    test_endpoint("8. Get Signed Messages", f"{BASE_URL}/signed")
    test_endpoint("9. Get Unsigned Messages", f"{BASE_URL}/unsigned")
    test_endpoint("10. Get Validated Messages", f"{BASE_URL}/validated")
    test_endpoint("11. Get Unvalidated Messages", f"{BASE_URL}/unvalidated")
    test_endpoint("12. Reset Table", f"{BASE_URL}/Reset")
