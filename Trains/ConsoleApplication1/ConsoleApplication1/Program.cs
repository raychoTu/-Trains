using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1 {
    enum CardType {
        Family,
        Elder,
        None
    }

    class Train {
        public string destination;
        public TimeSpan hourOfLeaving;
        public int seatsAvailable;

        public Train(string destination, TimeSpan hourOfLeaving, int seatsAvailable) {
            this.destination = destination;
            this.hourOfLeaving = hourOfLeaving;
            this.seatsAvailable = seatsAvailable;
        }
    }

    class TrainTicketsManager {
        private const Train[] trains = {new Train("New York", new TimeSpan(8, 30, 0), 50),
                                        new Train("Boston", new TimeSpan(10, 15, 0), 35),
                                        new Train("Washington D.C.", new TimeSpan(13, 45, 0), 70)};
        private const double standartPrice = 100;
        private double priceOfTicket = standartPrice;

        public double getPriceFor(string hourOfLeaving, CardType cardType, int ticketsCount, String destination, bool hasChild, bool isRoundTrip) {
            TimeSpan hour = TimeSpan.Parse(hourOfLeaving);

            if (isInRushHour(hour)) {
                return priceOfTicket;
            }

            switch (cardType) {
                case CardType.Family:
                    if (hasChild) {
                        priceOfTicket = priceOfTicket * 0.5;
                    }
                    else {
                        priceOfTicket = priceOfTicket * 0.9;
                    }
                    break;
                case CardType.Elder:
                    priceOfTicket = priceOfTicket * 0.66;
                    break;
            }

            calculateTicketPriceBasedOnTrip(isRoundTrip);

            return priceOfTicket * ticketsCount;
        }

        private bool isInRushHour(TimeSpan hour) {
            var morningStart = TimeSpan.Parse("07:30");
            var morningEnd = TimeSpan.Parse("09:30");
            var afterNoonStart = TimeSpan.Parse("16:00");
            var afterNoonEnd = TimeSpan.Parse("19:00");

            if ((hour >= morningStart && hour < morningEnd) 
                || (hour >= afterNoonStart && hour < afterNoonEnd)) {
                    return true;
            }
            return false;
        }

        private Double calculateTicketPriceBasedOnTrip(bool isRoundTrip) {
            if (isRoundTrip) {
                priceOfTicket *= 2;
                priceOfTicket *= 0.9; // 10% discount for round-trip
            }
            return priceOfTicket;
        }

        public bool reserveTicket(TimeSpan desiredTimeOfLeaving, int ticketsCount) {
            string message = "";
            bool isReservationSuccessful = false;

            foreach (Train train in trains) {
                // check if the train is available at the desired time
                if (train.hourOfLeaving == desiredTimeOfLeaving) {
                    // check if the train has enough seats
                    if (train.seatsAvailable >= ticketsCount) {
                        train.seatsAvailable -= ticketsCount;
                        message = "Reservation successful for " + ticketsCount + " seat(s) on train to {train.destination} leaving at " + train.hourOfLeaving;
                        isReservationSuccessful = true;
                        break;
                    }
                    else {
                        message = "Train to " + train.destination + " leaving at " + train.hourOfLeaving + " doesn't have enough seats.";
                        break;
                    }
                }
            }

            if (!isReservationSuccessful) {
                message = "No trains available at the desired time.";
            }
            
            Console.WriteLine(message);
            return isReservationSuccessful;
        }

        public TimeSpan getTrainsHourOfLeaving(string destination) {
            foreach (Train train in trains) {
                if (train.destination == destination) {
                    Console.WriteLine("Train to " + train.destination + "is leaving at " + train.hourOfLeaving);
                    return train.hourOfLeaving;
                }
            }
            Console.WriteLine("There are no trains with destination - " + destination);
            return new TimeSpan(0);
        }

    }

    class Program {

        static void Main(string[] args) {
            TrainTicketsManager manager = new TrainTicketsManager();

            double fullPrice = manager.getPriceFor("19:30", CardType.Elder, 1, "Tokyo", false, false);
            Console.WriteLine(fullPrice);
            Console.ReadKey();
        }
    }
}